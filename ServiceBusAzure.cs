using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections;




namespace Contmatic.Comuns.Infra.CrossCutting.ServiceBus
{
    /// <summary>
    /// Classe que representa objetos genericos send enviados para uma fila do Azure Service Bus
    /// </summary>
    public class ServiceBusAzure<T>
    {

        public ServiceBusAzure(String cCon, String queue)
        {
            Ccon = cCon;
            Queue = queue;
        }

       

        public string Ccon { get; private set; }

        public string Queue { get; private set; }

        public async void Post(IList lstSend)
        {
            try
            {
                if (lstSend != null)
                {
                    if (lstSend.Count > 0)
                    {
                        var nameSpace = NamespaceManager.CreateFromConnectionString(Ccon);

                        if (!nameSpace.QueueExists(Queue))
                        {
                            nameSpace.CreateQueue(Queue);
                        }

                        QueueClient fila =
                           QueueClient.CreateFromConnectionString(Ccon, Queue);

                        BrokeredMessage bkm = new BrokeredMessage(lstSend);
                        await fila.SendAsync(bkm);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Post(T objSend)
        {
            try
            {
                if (objSend != null)
                {                    
                        var nameSpace = NamespaceManager.CreateFromConnectionString(Ccon);

                        if (!nameSpace.QueueExists(Queue))
                        {
                            nameSpace.CreateQueue(Queue);
                        }

                        QueueClient fila =
                           QueueClient.CreateFromConnectionString(Ccon, Queue);

                        BrokeredMessage bkm = new BrokeredMessage(objSend);
                        fila.Send(bkm);                    
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
