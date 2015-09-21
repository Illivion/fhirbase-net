using System;
using System.Linq;
using Fhirbase.Net.Common;
using Fhirbase.Net.SearchHelpers;
using Hl7.Fhir.Model;
using Monads.NET;

namespace Fhirbase.Net.Api
{
    public class FhirStorage : IFhirStorage
    {
        #region :: CRUD ::

        public Resource Create(Resource entry)
        {
            var resourceJson = FHIRbaseHelper.FhirResourceToJson(entry);

            var resource = FHIRbase
                .Call(FhirSchema.Name, FhirSchema.Func.Create)
                .WithJsonb(resourceJson)
                .Cast<String>();

            return FHIRbaseHelper.JsonToFhirResource(resource);
        }

        public T Create<T>(T resource) where T : Resource
        {
            return (T)Create((Resource)resource);
        }

        public Resource Read(ResourceKey key)
        {
            var resourceJson = FHIRbase
                .Call(FhirSchema.Name, FhirSchema.Func.Read)
                .WithText(key.TypeName)
                .WithText(key.ResourceId)
                .Cast<string>();

            return FHIRbaseHelper.JsonToFhirResource(resourceJson);
        }

        public T Read<T>(ResourceKey key) where T : Resource
        {
            return (T)Read(key);
        }

        public Resource VRead(ResourceKey key)
        {
            var resourceJson = FHIRbase
                .Call(FhirSchema.Name, FhirSchema.Func.VRead)
                .WithText(key.TypeName)
                .WithText(key.VersionId)
                .Cast<string>();

            return FHIRbaseHelper.JsonToFhirResource(resourceJson);
        }

        public T VRead<T>(ResourceKey key) where T : Resource
        {
            return (T)VRead(key);
        }

        public Resource Update(Resource resource)
        {
            var resourceJson = FHIRbaseHelper.FhirResourceToJson(resource);

            var updatedResourceJson = FHIRbase
                .Call(FhirSchema.Name, FhirSchema.Func.Update)
                .WithJsonb(resourceJson)
                .Cast<string>();

            return FHIRbaseHelper.JsonToFhirResource(updatedResourceJson);
        }

        public T Update<T>(T resource) where T : Resource
        {
            return (T)Update((Resource)resource);
        }

        public Resource Delete(ResourceKey key)
        {
            var resourceJson = FHIRbase
                .Call(FhirSchema.Name, FhirSchema.Func.Delete)
                .WithText(key.TypeName)
                .WithText(key.ResourceId)
                .Cast<string>();

            return FHIRbaseHelper.JsonToFhirResource(resourceJson);
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
                parameters = new HistoryParameters();

            var historyResponse = FHIRbase.Call(FhirSchema.Name, FhirSchema.Func.History)
                .WithText(resourceType)
                .WithText(resourceId)
                .WithText(parameters.ToString())
                .Cast<string>();

            return FHIRbaseHelper.JsonToBundle(historyResponse);
        }

        public Bundle History(string resourceType, HistoryParameters parameters = null)
        {
            if (parameters == null)
                parameters = new HistoryParameters();

            var historyResponse = FHIRbase.Call(FhirSchema.Name, FhirSchema.Func.History)
                .WithText(resourceType)
                .WithText(parameters.ToString())
                .Cast<string>();

            return FHIRbaseHelper.JsonToBundle(historyResponse);
        }

        public Bundle History(HistoryParameters parameters = null)
        {
            if (parameters == null)
                parameters = new HistoryParameters();

            var historyResponse = FHIRbase.Call(FhirSchema.Name, FhirSchema.Func.History)
                .WithText(parameters.ToString())
                .Cast<string>();

            return FHIRbaseHelper.JsonToBundle(historyResponse);
        }

        #endregion

        #region :: Search ::
        public Bundle Search(string resource, SearchParameters parameters)
        {
            if (parameters == null)
                parameters = new SearchParameters();

            var searchQuery = parameters.ToString();
            var searchResult = FHIRbase.Call(FhirSchema.Name, FhirSchema.Func.Search)
                .WithText(resource)
                .WithText(searchQuery)
                .Cast<string>();

            return FHIRbaseHelper.JsonToBundle(searchResult);
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
            var result = FHIRbase.Call(FhirSchema.Name, FhirSchema.Func.GenTables)
                .WithTextArray(resources)
                .Cast<string>();

            return result;
        }

        /// <summary>
        /// Generate tables for DSTU2 resources
        /// </summary>
        /// <returns></returns>
        public String GenerateTables()
        {
            var result = FHIRbase.Call(FhirSchema.Name, FhirSchema.Func.GenTables)
                .Cast<string>();

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
            var conformanceResult = FHIRbase.Call(FhirSchema.Name, FhirSchema.Func.Conformance)
                .WithJsonb(cfg)
                .Cast<string>();

            return (Conformance)FHIRbaseHelper.JsonToFhirResource(conformanceResult);
        }

        public StructureDefinition StructureDefinition(string resourceName, string cfg = "{}")
        {
            var sdResult = FHIRbase.Call(FhirSchema.Name, FhirSchema.Func.StructureDefinition)
                .WithJsonb(cfg)
                .WithText(resourceName)
                .Cast<string>();

            return (StructureDefinition)FHIRbaseHelper.JsonToFhirResource(sdResult);
        }

        #endregion

        #region :: Transactions ::
        public Bundle Transaction(Bundle bundle)
        {
            var transactionJson = FHIRbaseHelper.FhirResourceToJson(bundle);
            var fhirbaseResult = FHIRbase.Call(FhirSchema.Name, FhirSchema.Func.Transaction)
                .WithJsonb(transactionJson)
                .Cast<string>();

            return FHIRbaseHelper.JsonToBundle(fhirbaseResult);
        }

        #endregion

        #region :: Indexing ::
        public String IndexSearchParam(string resource, string name)
        {
            var indexSearchParamsResult = FHIRbase.Call(FhirSchema.Name, FhirSchema.Func.IndexSearchParam)
                .WithText(resource)
                .WithText(name)
                .Cast<string>();

            return indexSearchParamsResult;
        }

        public Int64 DropIndexSearchParams(string resource, string name)
        {
            var dropIndexSearchParamsResult = FHIRbase.Call(FhirSchema.Name, FhirSchema.Func.DropIndexSearchParam)
                .WithText(resource)
                .WithText(name)
                .Cast<long>();

            return dropIndexSearchParamsResult;
        }

        public String[] IndexResource(string resource)
        {
            var indexResiurceResult = FHIRbase.Call(FhirSchema.Name, FhirSchema.Func.IndexResource)
                .WithText(resource)
                .Cast<string[]>();

            return indexResiurceResult;
        }

        public Int64 DropResourceIndexes(string resource)
        {
            var dropResourceIndexesResult = FHIRbase.Call(FhirSchema.Name, FhirSchema.Func.DropResourceIndexes)
                .WithText(resource)
                .Cast<long>();

            return dropResourceIndexesResult;
        }

        public String[] IndexAllResources()
        {
            var indexAllResourcesResult = FHIRbase.Call(FhirSchema.Name, FhirSchema.Func.IndexAllResources)
                .Cast<string[]>();

            return indexAllResourcesResult;
        }

        public Int64 DropAllResourceIndexes()
        {
            var dropAllResourceIndexesResult = FHIRbase.Call(FhirSchema.Name, FhirSchema.Func.DropAllResourceIndexes)
                .Cast<long>();

            return dropAllResourceIndexesResult;
        }

        #endregion

        #region :: Admin Functions ::
        public String AdminDiskUsageTop(int limit)
        {
            var adminDiskUsageTopResult = FHIRbase.Call(FhirSchema.Name, FhirSchema.Func.AdminDiskUsageTop)
                .WithInt(limit)
                .Cast<string>();

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
            return FHIRbase.Call(FhirSchema.Name, FhirSchema.Func.IsExists)
                .WithText(resourceName)
                .WithText(id)
                .Cast<Boolean>();
        }

        public Boolean IsDeleted(ResourceKey key)
        {
            return FHIRbase.Call(FhirSchema.Name, FhirSchema.Func.IsDeleted)
                .WithText(key.TypeName)
                .WithText(key.ResourceId)
                .Cast<bool>();
        }

        public Boolean IsLatest(ResourceKey key)
        {
            var result = FHIRbase.Call(FhirSchema.Name, FhirSchema.Func.IsLatest)
               .WithText(key.TypeName)
               .WithText(key.ResourceId)
               .WithText(key.VersionId)
               .Cast<bool>();

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
