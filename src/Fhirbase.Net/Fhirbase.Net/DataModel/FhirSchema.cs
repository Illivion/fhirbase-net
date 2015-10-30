using System;

namespace Fhirbase.Net.DataModel
{
    public static class FhirSchema
    {
        public static readonly String Name = "fhir";

        public static class Func
        {
            public static readonly String IsExists = "is_exists";

            public static readonly String IsDeleted = "is_deleted";

            public static readonly String IsLatest = "is_latest";

            public static readonly String Create = "create";

            public static readonly String Read = "read";

            public static readonly String VRead = "vread";

            public static readonly String Update = "update";

            public static readonly String Delete = "delete";

            public static readonly String History = "history";

            public static readonly String Search = "search";

            public static readonly String GenTables = "generate_tables";

            public static readonly String Conformance = "conformance";

            public static readonly String StructureDefinition = "structuredefinition";

            public static readonly String Transaction = "transaction";

            public static readonly String IndexSearchParam = "index_search_param";

            public static readonly String DropIndexSearchParam = "drop_index_search_param";

            public static readonly String IndexResource = "index_resource";

            public static readonly String DropResourceIndexes = "drop_resource_indexes";

            public static readonly String IndexAllResources = "index_all_resources";

            public static readonly String DropAllResourceIndexes = "drop_all_resource_indexes";

            public static readonly String AdminDiskUsageTop = "admin_disk_usage_top";
        }
    }
}