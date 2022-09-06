using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DifuntosApp
{
    class Program
    {
        static void Main(string[] args)
        { 
            

            DifuntosDBEntities difuntosDBEntities = new DifuntosDBEntities();
            
            DbContextTransaction dbCtxtTransaction = difuntosDBEntities.Database.BeginTransaction();
            
            List<SPGetDifuntos_Result> result2 = difuntosDBEntities.SPGetDifuntos().ToList();

            result2.ForEach(r => Console.WriteLine($"{r.Nombres} {r.Apellidos}\t{r.Documento}\t{r.Sexo}\t{r.FechaNacimiento}\t{r.Tipo}"));

            //difuntosDBEntities.tblDifunto.Add(d);
            //int cantidadDifuntosBefore = difuntosDBEntities.tblDifunto.Count();
            tblDifunto d = getDatosDifunto();
            int resultado = difuntosDBEntities.SPInsertDifunto(d.TipoDocumento, d.Documento, d.Nombres, d.Apellidos, d.FechaNacimiento, d.Sexo, d.FechaRegistro, d.Nota, d.Tipo, d.Estado);

           // Console.WriteLine($"Valor retornado por stored pocedure: {resultado}");
           // Console.ReadLine();

            //int cantidadDifuntosAfter = difuntosDBEntities.tblDifunto.Count();

            if (resultado == 1)
            {
                Console.WriteLine("\nDifunto registrado satisfactoriamente.");

                tblGeneral general = difuntosDBEntities.tblGeneral.SingleOrDefault(g => g.Id > 0);

                int cantidadDifuntos = difuntosDBEntities.tblDifunto.Count();

                if (object.ReferenceEquals(general, null))
                {
                    general = getDatosGeneral();
                    general.Cantidad = cantidadDifuntos;
                    difuntosDBEntities.tblGeneral.Add(general);
                }
                else
                {
                    general.Cantidad = cantidadDifuntos;
                    general.FchUltActualizacion = DateTime.Now;
                    difuntosDBEntities.Entry(general).State = EntityState.Modified;
                }

                if (difuntosDBEntities.SaveChanges() == 1)
                {
                    Console.WriteLine("\nRegistro general actualizado satisfactoriamente.");
                    dbCtxtTransaction.Commit();
                } else
                {
                    Console.WriteLine("\nEl registro general no se pudo crear o actualizar.");
                    dbCtxtTransaction.Rollback();
                }
            }

            //List<tblDifunto> read = difuntosDBEntities.Database.SqlQuery<tblDifunto>("SPGetDifuntos").ToList();
            List<SPGetDifuntos_Result> result = difuntosDBEntities.SPGetDifuntos().ToList();

            result.ForEach(r => Console.WriteLine($"{r.Nombres} {r.Apellidos}\t{r.Documento}\t{r.Sexo}\t{r.FechaNacimiento}\t{r.Tipo}"));

            Console.ReadLine();
            return;
        }

        static tblDifunto getDatosDifunto()
        {
            char TipoDocumento, Sexo, Estado;
            string Documento, Nombres, Apellidos, Nota, Tipo;
            DateTime FechaNacimiento;

            Console.WriteLine("\nTipo de documento (C, P, L): ");
            TipoDocumento = Console.ReadLine().ToCharArray()[0];

            Console.WriteLine("\nNo. de documento: ");
            Documento = Console.ReadLine();

            Console.WriteLine("\nNombres: ");
            Nombres = Console.ReadLine();

            Console.WriteLine("\nApellidos: ");
            Apellidos = Console.ReadLine();

            Console.WriteLine("\nFecha de nacimiento (dd-MM-yyyy): ");

            while (!DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out FechaNacimiento))
            {
                Console.WriteLine("\nFecha inválida, trate de nuevo: ");
            }

            Console.WriteLine("\nSexo (M/F): ");
            Sexo = Console.ReadLine().ToCharArray()[0];

            Console.WriteLine("\nNota: ");
            Nota = Console.ReadLine();

            Console.WriteLine("\nTipo de muerte: ");
            Tipo = Console.ReadLine();

            Console.WriteLine("\nEstado(0/1): ");
            Estado = Console.ReadLine().ToCharArray()[0];

            return new tblDifunto
            {
                TipoDocumento = TipoDocumento.ToString(),
                Documento = Documento,
                Nombres = Nombres,
                Apellidos = Apellidos,
                FechaNacimiento = FechaNacimiento,
                FechaRegistro = DateTime.Now,
                Sexo = Sexo.ToString(),
                Nota = Nota,
                Tipo = Tipo,
                Estado = Estado.ToString()
            };
        }

        private static tblGeneral getDatosGeneral()
        {
            tblGeneral general = new tblGeneral();

            Console.WriteLine("\n\nInserte datos para tabla general:");

            Console.WriteLine("\nDescripción: ");
            general.Descripcion = Console.ReadLine();

            general.FchUltActualizacion = DateTime.Now;

            Console.WriteLine("\nEstado: ");
            general.Estado = Console.ReadLine();

            return general;
        }

        private static tblDifunto TestInsetDifunto()
        {
            //tblDifunto difunto = new tblDifunto
            return new tblDifunto
            {
                TipoDocumento = "Cedula",
                Documento = "40219326887",
                Nombres = "Fernando",
                Apellidos = "Rosa",
                FechaNacimiento = DateTime.Now,
                FechaRegistro = DateTime.Now,
                Sexo = "M",
                Nota = "Se encuentra en el cielo",
                Tipo = "Decapitación",
                Estado = "Activo"
            };
            //DifuntosDBEntities difuntosDBEntities = new DifuntosDBEntities();

            //difuntosDBEntities.tblDifunto.Add(difunto);
            //return difuntosDBEntities.SaveChanges();
        }
    }
}
