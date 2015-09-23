using System;
using System.Configuration;
using System.Linq;
using Fhirbase.Net.Common;
using Fhirbase.Net.DataModel;
using Fhirbase.Net.Helpers;
using Hl7.Fhir.Model;
using Monads.NET;

namespace Fhirbase.Net.Api
{
    public class FhirStorage : IFhirStorage
    {
        private readonly FhirbaseContext _context;

        public FhirStorage(string nameOrConnectionString)
        {
            var connectionString = GetConnectionString(nameOrConnectionString);

            _context = new FhirbaseContext(connectionString);
        }

        private string GetConnectionString(string nameOrConnectionString)
        {
            var connectionString = "";

            if (nameOrConnectionString.IndexOf('=') >= 0)
            {
                connectionString = nameOrConnectionString;
            }
            else
            {
                connectionString = ConfigurationManager.ConnectionStrings[nameOrConnectionString].ConnectionString;
            }
            return connectionString;
        }

        #region :: CRUD ::

        public Resource Create(Resource entry)
        {
            var resourceJson = ResourceDataHelper.FhirResourceToJson(entry);

            var createdResourceJson = _context
                .Call(FhirSchema.Name, FhirSchema.Func.Create)
                .WithJson(resourceJson)
                .Cast<String>();

            var createdResource = ResourceDataHelper.JsonToFhirResource(createdResourceJson);

            return createdResource;
        }

        public T Create<T>(T resource) where T : Resource
        {
            return (T)Create((Resource)resource);
        }

        public Resource Read(ResourceKey key)
        {
            var resourceJson = _context
                .Call(FhirSchema.Name, FhirSchema.Func.Read)
                .WithString(key.TypeName)
                .WithString(key.ResourceId)
                .Cast<string>();

            var resource = ResourceDataHelper.JsonToFhirResource(resourceJson);

            return resource;
        }

        public T Read<T>(ResourceKey key) where T : Resource
        {
            return (T)Read(key);
        }

        public Resource VRead(ResourceKey key)
        {
            var resourceJson = _context
                .Call(FhirSchema.Name, FhirSchema.Func.VRead)
                .WithString(key.TypeName)
                .WithString(key.VersionId)
                .Cast<string>();

            var resource = ResourceDataHelper.JsonToFhirResource(resourceJson);

            return resource;
        }

        public T VRead<T>(ResourceKey key) where T : Resource
        {
            return (T)VRead(key);
        }

        public Resource Update(Resource resource)
        {
            var resourceJson = ResourceDataHelper.FhirResourceToJson(resource);

            var updatedResourceJson = _context
                .Call(FhirSchema.Name, FhirSchema.Func.Update)
                .WithJson(resourceJson)
                .Cast<string>();

            var updatedResource = ResourceDataHelper.JsonToFhirResource(updatedResourceJson);

            return updatedResource;
        }

        public T Update<T>(T resource) where T : Resource
        {
            return (T)Update((Resource)resource);
        }

        public Resource Delete(ResourceKey key)
        {
            var resourceJson = _context
                .Call(FhirSchema.Name, FhirSchema.Func.Delete)
                .WithString(key.TypeName)
                .WithString(key.ResourceId)
                .Cast<String>();

            var deletedResource = ResourceDataHelper.JsonToFhirResource(resourceJson);

            return deletedResource;
        }

        public T Delete<T>(ResourceKey key) where T : Resource
        {
            return (T)Delete(key);
        }

        #endregion

        #region :: History :: 
        
        /// <summary>
        /// Retrieve the update history for a particular resource
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceId"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Bundle History(string resourceType, string resourceId, HistoryParameters parameters = null)
        {
            if (parameters == null)
            {
                parameters = HistoryParameters.Empty;
            }

            var historyResponse = _context.Call(FhirSchema.Name, FhirSchema.Func.History)
                .WithString(resourceType)
                .WithString(resourceId)
                .WithString(parameters.ToString())
                .Cast<String>();

            var resultBundle = ResourceDataHelper.JsonToBundle(historyResponse);

            return resultBundle;
        }

        public Bundle History(string resourceType, HistoryParameters parameters = null)
        {
            if (parameters == null)
            {
                parameters = HistoryParameters.Empty;
            }

            var historyResponse = _context.Call(FhirSchema.Name, FhirSchema.Func.History)
                .WithString(resourceType)
                .WithString(parameters.ToString())
                .Cast<String>();

            var resultBundle = ResourceDataHelper.JsonToBundle(historyResponse);

            return resultBundle;
        }

        public Bundle History(HistoryParameters parameters = null)
        {
            if (parameters == null)
            {
                parameters = HistoryParameters.Empty;
            }

            var historyResponse = _context.Call(FhirSchema.Name, FhirSchema.Func.History)
                .WithString(parameters.ToString())
                .Cast<String>();

            var resultBundle = ResourceDataHelper.JsonToBundle(historyResponse);

            return resultBundle;
        }

        #endregion

        #region :: Search ::

        public Bundle Search(string resource, SearchParameters parameters)
        {
            if (parameters == null)
            {
                parameters = SearchParameters.Empty;
            }

            var searchQuery = parameters.ToString();

            var searchResult = _context.Call(FhirSchema.Name, FhirSchema.Func.Search)
                .WithString(resource)
                .WithString(searchQuery)
                .Cast<String>();

            var resultBundle = ResourceDataHelper.JsonToBundle(searchResult);

            return resultBundle;
        }

        #endregion

        #region :: Generation ::

        /// <summary>
        /// Generate tables for resources
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        public String GenerateTables(params string[] resources)
        {
            var result = _context.Call(FhirSchema.Name, FhirSchema.Func.GenTables)
                .WithStringArray(resources)
                .Cast<String>();

            return result;
        }

        /// <summary>
        /// Generate tables for DSTU2 resources
        /// </summary>
        /// <returns></returns>
        public String GenerateTables()
        {
            var result = _context.Call(FhirSchema.Name, FhirSchema.Func.GenTables)
                .Cast<String>();

            return result;
        }

        #endregion

        #region :: Structure ::

        /// <summary>
        /// Create FHIR-conformance
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns>JSON with conformance</returns>
        public Conformance Conformance(string cfg = "{}")
        {
            var conformanceResult = _context.Call(FhirSchema.Name, FhirSchema.Func.Conformance)
                .WithJson(cfg)
                .Cast<String>();

            var conformanceResource = (Conformance)ResourceDataHelper.JsonToFhirResource(conformanceResult);

            return conformanceResource;
        }

        public StructureDefinition StructureDefinition(string resourceName, string cfg = "{}")
        {
            var sdResult = _context.Call(FhirSchema.Name, FhirSchema.Func.StructureDefinition)
                .WithJson(cfg)
                .WithString(resourceName)
                .Cast<String>();

            var structureDefinitionResource = (StructureDefinition)ResourceDataHelper.JsonToFhirResource(sdResult);

            return structureDefinitionResource;
        }

        #endregion

        #region :: Transactions ::

        public Bundle Transaction(Bundle bundle)
        {
            var transactionJson = ResourceDataHelper.FhirResourceToJson(bundle);

            var fhirbaseResult = _context.Call(FhirSchema.Name, FhirSchema.Func.Transaction)
                .WithJson(transactionJson)
                .Cast<String>();

            var transactionResult = ResourceDataHelper.JsonToBundle(fhirbaseResult);

            return transactionResult;
        }

        #endregion

        #region :: Indexing ::

        public String IndexSearchParam(string resource, string name)
        {
            var indexSearchParamsResult = _context.Call(FhirSchema.Name, FhirSchema.Func.IndexSearchParam)
                .WithString(resource)
                .WithString(name)
                .Cast<String>();

            return indexSearchParamsResult;
        }

        public Int64 DropIndexSearchParams(string resource, string name)
        {
            var dropIndexSearchParamsResult = _context.Call(FhirSchema.Name, FhirSchema.Func.DropIndexSearchParam)
                .WithString(resource)
                .WithString(name)
                .Cast<Int64>();

            return dropIndexSearchParamsResult;
        }

        public String[] IndexResource(string resource)
        {
            var indexResiurceResult = _context.Call(FhirSchema.Name, FhirSchema.Func.IndexResource)
                .WithString(resource)
                .Cast<String[]>();

            return indexResiurceResult;
        }

        public Int64 DropResourceIndexes(string resource)
        {
            var dropResourceIndexesResult = _context.Call(FhirSchema.Name, FhirSchema.Func.DropResourceIndexes)
                .WithString(resource)
                .Cast<Int64>();

            return dropResourceIndexesResult;
        }

        public String[] IndexAllResources()
        {
            var indexAllResourcesResult = _context.Call(FhirSchema.Name, FhirSchema.Func.IndexAllResources)
                .Cast<String[]>();

            return indexAllResourcesResult;
        }

        public Int64 DropAllResourceIndexes()
        {
            var dropAllResourceIndexesResult = _context.Call(FhirSchema.Name, FhirSchema.Func.DropAllResourceIndexes)
                .Cast<Int64>();

            return dropAllResourceIndexesResult;
        }

        #endregion

        #region :: Admin Functions ::

        public String AdminDiskUsageTop(int limit)
        {
            var adminDiskUsageTopResult = _context.Call(FhirSchema.Name, FhirSchema.Func.AdminDiskUsageTop)
                .WithInt32(limit)
                .Cast<String>();

            return adminDiskUsageTopResult;
        }

        #endregion

        #region :: Resource Utility ::

        public Boolean IsExists(ResourceKey key)
        {
            return IsExists(key.TypeName, key.ResourceId);
        }

        public Boolean IsExists(Resource entry)
        {
            return entry != null
                && !String.IsNullOrEmpty(entry.TypeName)
                && !String.IsNullOrEmpty(entry.Id)
                && IsExists(entry.TypeName, entry.Id);
        }

        public Boolean IsExists(string resourceName, string id)
        {
            return _context.Call(FhirSchema.Name, FhirSchema.Func.IsExists)
                .WithString(resourceName)
                .WithString(id)
                .Cast<Boolean>();
        }

        public Boolean IsDeleted(ResourceKey key)
        {
            return _context.Call(FhirSchema.Name, FhirSchema.Func.IsDeleted)
                .WithString(key.TypeName)
                .WithString(key.ResourceId)
                .Cast<Boolean>();
        }

        public Boolean IsLatest(ResourceKey key)
        {
            var result = _context.Call(FhirSchema.Name, FhirSchema.Func.IsLatest)
               .WithString(key.TypeName)
               .WithString(key.ResourceId)
               .WithString(key.VersionId)
               .Cast<Boolean>();

            return result;
        }

        public Resource ReadLastVersion(ResourceKey key)
        {
            var lastVersion = History(key.TypeName, key.ResourceId)
                .With(x => x.Entry)
                .Select(x => x.Resource)
                .Where(x => x.HasVersionId)
                .OrderBy(x => x.Meta.LastUpdated)
                .LastOrDefault();

            return lastVersion;
        }

        #endregion
    }
}
