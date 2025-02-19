﻿using System.Collections.Generic;

namespace WypozyczalaniaProjekt.DAL.Repozytoria
{
    using Encje;
    using MySql.Data.MySqlClient;
    class RepozytoriumOddzialy
    {
        #region ZAPYTANIA

        private const string WSZYSTKIE_ODDZIALY = "SELECT * FROM oddzialy order by id_oddzialu asc";
        private const string DODAJ_ODDZIAL = "INSERT INTO oddzialy (adres, nr_telefonu, nazwa) VALUES";

        #endregion

        #region metody CRUD

        public static List<Oddzial> PobierzWszystkieOddzialy(IDBConnection database)
        {
            List<Oddzial> oddzialy = new List<Oddzial>();

            using (var connection = database.GetConnection())
            {
                MySqlCommand command = new MySqlCommand(WSZYSTKIE_ODDZIALY, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                    oddzialy.Add(new Oddzial(reader));
                connection.Close();
            }
            return oddzialy;
        }

        public static bool DodajOddzialDoBazy(IDBConnection database, Oddzial oddzial)
        {
            bool stan = false;
            using (var connection = database.GetConnection())
            {
                MySqlCommand command = new MySqlCommand($"{DODAJ_ODDZIAL} {oddzial.ToInsert()}", connection);
                connection.Open();
                var add = command.ExecuteNonQuery();
                stan = true;
                oddzial.IdOddzialu = (sbyte)command.LastInsertedId;
                connection.Close();
            }
            return stan;
        }

        public static bool EdytujOddzialWBazie(IDBConnection database, Oddzial od, sbyte idOddzialu)
        {
            bool stan = false;
            using (var connenction = database.GetConnection())
            {
                string EDYTUJ_ODDZIAL = $"UPDATE oddzialy SET adres='{od.Adres}',nr_telefonu='{od.NrTelefonu}', nazwa='{od.Nazwa}' WHERE id_oddzialu='{idOddzialu}'";

                MySqlCommand command = new MySqlCommand(EDYTUJ_ODDZIAL, connenction);
                connenction.Open();
                var edit = command.ExecuteNonQuery();
                if (edit == 1) stan = true;
                connenction.Close();
            }
            return stan;
        }

        public static bool UsunOddzialZBazy(IDBConnection database, sbyte idOddzialu)
        {
            bool stan = false;
            using (var connection = database.GetConnection())
            {
                string USUN_ODDZIAL = $"DELETE FROM oddzialy WHERE id_oddzialu={idOddzialu}";

                MySqlCommand command = new MySqlCommand(USUN_ODDZIAL, connection);
                connection.Open();
                var delete = command.ExecuteNonQuery();
                if (delete == 1) stan = true;
                connection.Close();
            }
            return stan;
        }

        #endregion
    }
}
