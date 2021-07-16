using System;
using System.IO;
using System.Runtime;
using System.Threading;

namespace Core_Facilities
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //1.Пример обработки исключений любых двух примитивных типов(соответственно должно быть два блока catch + finally)
            FileStream fs = null;
            var open = FileMode.Open;
            int a;
            try
            {
                fs = new FileStream("file.txt", open);
                a = int.Parse("");
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error: {e}");
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Error: {e}");
            }
            finally
            {
                if (fs != null) fs.Close();
            }
            //2.Пример собственного типа исключения + обработка выброса такого исключения
            try
            {
                throw new NonTextFileException("file.cvs");
            }
            catch (NonTextFileException ex)
            {
                Console.WriteLine(ex.Message);
            }
            //3.Любой пример работы с GC(2 - 3 метода).
            Timer t = new Timer(TimerCallback, null, 0, 2000);
            Console.ReadLine();
            t.Dispose();
            Console.WriteLine("Application is running with server GC=" + GCSettings.IsServerGC);
            Console.WriteLine("Application is running with GC mode=" + GCSettings.LatencyMode);
            var str = "string";
            Console.WriteLine("Total memory = " + GC.GetTotalMemory(false));
            Console.WriteLine("Str generation = " + GC.GetGeneration(str));

            

            //5.Пример работы с инструкцией using
            using (FileStream file = new FileStream("file.txt", FileMode.OpenOrCreate))
            {
                byte[] text = System.Text.Encoding.Default.GetBytes("text");
                file.Write(text);
            }
            File.Delete("file.txt");
        }

        private static void TimerCallback(object o)
        {
            Console.WriteLine("In TimerCallback: " + DateTime.Now);
            GC.Collect();
        }
    }

    public sealed class NonTextFileException: Exception //Пример собственного типа исключения
        {
            private string _file;
            public NonTextFileException(string fileName)
            {
                _file = fileName[fileName.IndexOf('.')..];
            }
            public override string Message
            {
                get 
                {
                    return _file == null ? base.Message : $"{_file} - not a text file extension";
                } 
            }
    }
    //4.Пример работы с IDisposable.
    public class NewClass : IDisposable
    {
        private bool IsDisposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed) return;
            if (disposing)
            {
                Console.WriteLine("Disposed");
            }
            IsDisposed = true;
        }
        ~NewClass()
        {
            Dispose(false);
        }
    }

}

