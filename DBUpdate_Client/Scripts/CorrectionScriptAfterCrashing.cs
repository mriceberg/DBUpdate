using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUpdate_Client
{
    public class CorrectionScriptAfterCrashing
    {
        private readonly DBUpdateParameters _parameters;
        private readonly IConnectionProvider _connectionProvider;

        public CorrectionScriptAfterCrashing(DBUpdateParameters parameters, IConnectionProvider connectionProvider)
        {
            this._parameters = parameters;
            this._connectionProvider = connectionProvider;
        }

        public void RemoveScriptInTrackingTable()
        {
            ScriptGateway scriptGateway = new ScriptGateway(_connectionProvider);
            IEnumerable<string> executedScriptName = scriptGateway.GetExecutedScriptNames();

            //TODO : DbUpdateExecutionDescriptorProcessor Retirer tout dans blocktoexecute si paramètre qui spécifie un crash avec le nom du block à réexécuter.
            //TODO : Ajouter paramètre pour un crash et spécifier le nom du block et du script a partir du quel il faut repartir
            
        }
    }
}
