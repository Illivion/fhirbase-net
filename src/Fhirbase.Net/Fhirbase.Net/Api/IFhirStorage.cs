using System;
using Fhirbase.Net.Common;
using Fhirbase.Net.SearchHelpers;
using Hl7.Fhir.Model;

namespace Fhirbase.Net.Api
{
    /// <summary>
    /// Fhirbase RESTful+ API
    /// </summary>
    /// <exception cref="FHIRbaseException"></exception>
    public interface IFhirStorage
    {
        #region :: Generation ::

        /// <summary>
        /// Generate tables for resources
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        String GenerateTables(params string[] resources);

        /// <summary>
        /// Generate tables for DSTU2 resources
        /// </summary>
        /// <returns></returns>
        String GenerateTables();

        #endregion

        #region :: CRUD ::

        /// <summary>
        /// Read the current state of the resource
        /// </summary>
        /// <param name="key">[type] [id]</param>
        /// <returns></returns>
        Resource Read(ResourceKey key);

        /// <summary>
        /// Read the state of a specific version of the resource
        /// </summary>
        /// <param name="key">[type] [id] [vid]</param>
        /// <returns></returns>
        Resource VRead(ResourceKey key);

        /// <summary>
        /// Update an existing resource by its id (or create it if it is new)
        /// </summary>
        /// <param name="resource">[type] [id]</param>
        /// <returns></returns>
        Resource Update(Resource resource);

        /// <summary>
        /// Delete a resource
        /// </summary>
        /// <param name="key">[type] [id]</param>
        /// <returns></returns>
        Resource Delete(ResourceKey key);

        /// <summary>
        /// Create a new resource with a server assigned id
        /// </summary>
        /// <param name="resource">[type]</param>
        /// <returns></returns>
        Resource Create(Resource resource);

        #endregion

        #region :: History ::

        /// <summary>
        /// Retrieve the update history for a particular resource
        /// </summary>
        /// <param name="parameters">count and/or since</param>
        /// <returns></returns>
        Bundle History(HistoryParameters parameters = null);

        /// <summary>
        /// Retrieve the update history for a particular resource
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="parameters">count and/or since</param>
        /// <returns></returns>
        Bundle History(string resourceType, HistoryParameters parameters = null);

        /// <summary>
        /// Retrieve the update history for a particular resource
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceId"></param>
        /// <param name="parameters">count and/or since</param>
        /// <returns></returns>
        Bundle History(string resourceType, string resourceId, HistoryParameters parameters = null);

        #endregion

        #region :: Resource Utility ::

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Boolean IsExists(ResourceKey key);

        Boolean IsExists(Resource resource);

        Boolean IsExists(string resourceName, string id);

        Boolean IsDeleted(ResourceKey key);

        /// <summary>
        /// Check resource is latest version
        /// </summary>
        /// <param name="key">[type] [id] [vid]</param>
        /// <returns></returns>
        Boolean IsLatest(ResourceKey key);

        Resource ReadLastVersion(ResourceKey key);

        #endregion

        #region :: Search ::

        /// <summary>
        /// Search the resource type based on some filter criteria
        /// </summary>
        /// <param name="resource">Resource Type</param>
        /// <param name="parameters">Name-value set</param>
        /// <returns></returns>
        Bundle Search(string resource, SearchParameters parameters = null);


        #endregion

        #region :: Conformance ::

        /// <summary>
        /// Create FHIR-conformance
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns>Conformance</returns>
        Conformance Conformance(string cfg = "{}");

        StructureDefinition StructureDefinition(string resourceName, string cfg = "{}");

        #endregion

        #region :: Transactions ::

        /// <summary>
        /// Update, create or delete a set of resources as a single transaction
        /// </summary>
        /// <param name="bundle"></param>
        /// <returns></returns>
        Bundle Transaction(Bundle bundle);

        #endregion

        #region :: Indexing ::

        String IndexSearchParam(string resource, string name);

        Int64 DropIndexSearchParams(string resource, string name);

        String[] IndexResource(string resource);

        Int64 DropResourceIndexes(string resource);

        String[] IndexAllResources();

        Int64 DropAllResourceIndexes();

        #endregion

        #region :: Admin Disk Functions ::

        String AdminDiskUsageTop(int limit);

        #endregion
    }
}
