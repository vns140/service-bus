
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Receive();
        }

        private static void Sender()
        {
            string cCon = "Endpoint=sb://kasolution.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=WK3/ZGFGl9duyTWuWitZfwMqK3ebuVGXZ7RsxFMGZFA=";


            var nameSpace = NamespaceManager.CreateFromConnectionString(cCon);

            if (!nameSpace.QueueExists("NotaFiscal"))
            {
                nameSpace.CreateQueue("NotaFiscal");
            }

            QueueClient fila =
               QueueClient.CreateFromConnectionString(cCon, "NotaFiscal");

            int numero = 0;
            while (numero <= 20)
            {
                numero++;
                System.Threading.Thread.Sleep(3000);
                NotaFiscal nota = new NotaFiscal
                {
                    Apelido = "APODO" + numero,
                    CNPJ_CPF = numero.ToString("00.000.000/000-00"),
                    DataNota = DateTime.Now,
                    Valor = numero * 3,
                    Imposto = Convert.ToDecimal((numero * 3) * 0.10),
                    NumeroNf = numero

                };

                BrokeredMessage bkm = new BrokeredMessage(nota);
                fila.Send(bkm);
            }
        }
        private static void Receive()
        {
            string cCon = "Endpoint=sb://kasolution.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=WK3/ZGFGl9duyTWuWitZfwMqK3ebuVGXZ7RsxFMGZFA=";

            QueueClient fila =
               QueueClient.CreateFromConnectionString(cCon, "NotaFiscal");


            for (var r = fila.Receive(); r != null; r = fila.Receive())
            {

                NotaFiscal nf = r.GetBody<NotaFiscal>();

                if (nf != null)
                {
                    Console.WriteLine("----------------------------------------------");
                    Console.WriteLine("Numero: " + nf.NumeroNf);
                    Console.WriteLine("Data: " + nf.DataNota);
                    Console.WriteLine("Valor" + nf.Valor);
                    Console.WriteLine("Imposto" + nf.Imposto);
                    Console.WriteLine("CNPJ" + nf.CNPJ_CPF);
                    Console.WriteLine("_____________________________________________");
                    r.Complete();
                }

            }
        }


    }
}
