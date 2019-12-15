using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Envanter3
{

    class Program
    {
        static void Main(string[] args)
        {
            int müşteriNumarası = 15000000;

            ÇalıştırmaMotoru.KomutÇalıştır("MuhasebeModulu", "MaaşYatır", new object[] { müşteriNumarası });

            ÇalıştırmaMotoru.KomutÇalıştır("MuhasebeModulu", "YıllıkÜcretTahsilEt", new object[] { müşteriNumarası });

            ÇalıştırmaMotoru.BekleyenİşlemleriGerçekleştir();

            Console.ReadKey();
        }
    }

    public class ÇalıştırmaMotoru
    {
        private static Queue<Job> queue = new Queue<Job>();

        public static object KomutÇalıştır(string modülSınıfAdı, string methodAdı, object[] inputs)
        {
            Veritabanıİşlemleri db = new Veritabanıİşlemleri();
            var job = db.GetJobByModuleClassNameAndMethodName(modülSınıfAdı, methodAdı);

            if (job == null)
            {
                return InvokeMethod(modülSınıfAdı, methodAdı, inputs);
            }
            else
            {
                queue.Enqueue(job);
                return null;
            }
        }

        public static void BekleyenİşlemleriGerçekleştir()
        {
            // Bekleyen işlemlerin hepsini aynı anda tekrar devreye aldığımız zaman sistemde yük oluşturabilir. 
            // Bu açından kullanıcı eğer isterse zamanlanmış tarihte operasyonu çalıştırabilecek bir özellik eklendi.
            while (queue.Count > 0)
            {
                var job = queue.Dequeue();

                // Zamanlanmış iş geçtiyse hemen invoke edilir. Kullanıcı isteğine göre çalıştırılmayada bilir . Bu şekilde kurguladım.
                if (job.CalistirmaZamani <= DateTime.Now)
                {
                    Task.Run(() => InvokeMethod(job.ModulSinifAdi, job.MethodAdi, job.GetObjectParameters()));
                }
                else
                {
                    TaskScheduler.Instance.
                        ScheduleTask(job.CalistirmaZamani, () => InvokeMethod(job.ModulSinifAdi, job.MethodAdi, job.GetObjectParameters()));
                }

            }
        }

        private static object InvokeMethod(string modülSınıfAdı, string methodAdı, object[] inputs)
        {
            //Şuan örnekte başka dll olmadığı için böyle yaptım. Başka dll'de olsaydı Assembly'i load etmem gerekiyordu.

            var type = typeof(ÇalıştırmaMotoru).Assembly.GetTypes().FirstOrDefault(a => a.Name == modülSınıfAdı);
            var methodInfo = type.GetMethod(methodAdı, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return methodInfo.Invoke(Activator.CreateInstance(type), inputs);

        }
    }

    public class MuhasebeModulu
    {
        private void MaaşYatır(int müşteriNumarası)
        {
            // gerekli işlemler gerçekleştirilir.
            Console.WriteLine(string.Format("{0} numaralı müşterinin maaşı yatırıldı.", müşteriNumarası));
        }

        private void YıllıkÜcretTahsilEt(int müşteriNumarası)
        {
            // gerekli işlemler gerçekleştirilir.
            Console.WriteLine("{0} numaralı müşteriden yıllık kart ücreti tahsil edildi.", müşteriNumarası);
        }

        private void OtomatikÖdemeleriGerçekleştir(int müşteriNumarası)
        {
            // gerekli işlemler gerçekleştirilir.
            Console.WriteLine("{0} numaralı müşterinin otomatik ödemeleri gerçekleştirildi.", müşteriNumarası);
        }
    }

    public class Veritabanıİşlemleri
    {
        public Job GetJobByModuleClassNameAndMethodName(string ModulSinifAdi, string MethodAdi)
        {
            List<Job> jobs = new List<Job>();

            jobs.Add(new Job()
            {
                Id = 1,
                MethodAdi = "MaaşYatır",
                ModulSinifAdi = "MuhasebeModulu",
                CalistirmaZamani = new DateTime(2019, 12, 12, 12, 01, 000),
                Parameters = new List<JobParameter>() { new JobParameter() { JobId = 1, OrderNo = 1, ParameterType = ParameterTypes.Number, ParameterValue = "15000000" } }
            });

            return jobs.Where(a => a.ModulSinifAdi == ModulSinifAdi && a.MethodAdi == MethodAdi).SingleOrDefault();
        }
    }
}
