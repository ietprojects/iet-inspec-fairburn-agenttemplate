using IET.Inspec.Fairburn.Framework;
using IET.Inspec.Fairburn.AgentTemplate.Contract;
using System;
using System.Data.SqlClient;

namespace IET.Inspec.Fairburn.AgentTemplate
{

    public class Service : IService, IDisposable
    {

        private WindowsService _Service;
        private DatabaseConnection _Db;

        #region Constructors

        public Service()
        {
            _Service = AppDomain.CurrentDomain.GetData("FairburnWindowsService") as WindowsService;
            string connectionString = AppSettings.GetString("AgentTemplateDbConnectionString");
            int commandTimeoutSeconds = AppSettings.GetInt32("AgentTemplateDbCommandTimeoutSeconds", 30);
            _Db = new DatabaseConnection(connectionString, commandTimeoutSeconds);
        }

        #endregion

        #region Properties

        public string EndpointName
        {
            get
            {
                return "InternalEndoint";
            }
        }

        public string EndpointAddress
        {
            get
            {
                return "Internal";
            }
        }

        #endregion

        #region Methods

        public AgentPingResponse Ping()
        {
            return InvokeAndCheck<AgentPingResponse>(() => DoPing(), "PingGetDatabaseInfo");
        }

        public TaskResponse GetTask(GetTaskRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");
            return InvokeAndCheck<TaskResponse>(() => DoGetTask(request), "AgentTemplateGetTask");
        }

        public AgentAcknowledgeResponse PostResults(PostResultsRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");
            return InvokeAndCheck<AgentAcknowledgeResponse>(() => DoPostResults(request), "AgentTemplatePostResults");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        #endregion

        #region Private

        private AgentPingResponse DoPing()
        {
            using (DatabaseRequest db = new DatabaseRequest(_Db, "PingGetDatabaseInfo"))
            {
                using (SqlDataReader reader = db.ExecuteSingleReader())
                {
                    AgentPingResponse response = new AgentPingResponse();
                    reader.Read();
                    if (!reader.IsDBNull(0)) response.Timestamp = reader.GetDateTime(0);
                    if (!reader.IsDBNull(1)) response.ServerInstance = reader.GetString(1);
                    if (!reader.IsDBNull(2)) response.Version = reader.GetString(2);
                    if (!reader.IsDBNull(3)) response.Edition = reader.GetString(3);
                    if (!reader.IsDBNull(4)) response.Database = reader.GetString(4);
                    if (!reader.IsDBNull(5)) response.User = reader.GetString(5);
                    return response;
                }
            }
        }

        private TaskResponse DoGetTask(GetTaskRequest request)
        {
            using (DatabaseRequest db = new DatabaseRequest(_Db, "AgentTemplateGetTask"))
            {
                if (request.TestMode != 0) db.AddParameter("@TestMode", request.TestMode);
                using (SqlDataReader reader = db.ExecuteSingleReader())
                {
                    if (!reader.Read()) return null;
                    TaskResponse response = new TaskResponse();
                    if (!reader.IsDBNull(0)) response.Lock = reader.GetGuid(0);
                    return response;
                }
            }
        }

        private AgentAcknowledgeResponse DoPostResults(PostResultsRequest request)
        {
            using (DatabaseRequest db = new DatabaseRequest(_Db, "AgentTemplatePostResults"))
            {
                db.AddParameter("@Lock", request.Lock);
                if (request.TestMode != 0) db.AddParameter("@TestMode", request.TestMode);
                db.ExecuteNonQuery();
                AgentAcknowledgeResponse response = new AgentAcknowledgeResponse();
                return response;
            }
        }

        private TFunc InvokeAndCheck<TFunc>(Func<TFunc> method, string storedProcedure)
        {
            if (method == null) throw new ArgumentNullException("method");
            if (storedProcedure == null) throw new ArgumentNullException("storedProcedure");
            try
            {
                return method.Invoke();
            }
            catch (Exception ex)
            {
                if (_Service != null)
                {
                    string reason = ex.Message;
                    if (string.IsNullOrEmpty(reason)) reason = "Reason unknown.";
                    _Service.LogWarning(storedProcedure + " failed - " + reason + Environment.NewLine + ex.ToString());
                }
                throw;
            }
        }

        #endregion

    }

}
