﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WypozyczalaniaProjekt.DAL.Repozytoria
{
    using Encje;
    using MySql.Data.MySqlClient;
    class RepozytoriumKategorie
    {
        #region ZAPYTANIA

        private const string WSZYSTKIE_KATEGORIE = "SELECT * FROM kategoria order by nazwa asc";

        #endregion

        #region metody CRUD

        public static List<Kategoria> PobierzWszystkieKategorie()
        {
            List<Kategoria> kategorie = new List<Kategoria>();

            using (var connection = DBConnection.Instance.Connection)
            {
                MySqlCommand command = new MySqlCommand(WSZYSTKIE_KATEGORIE, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                    kategorie.Add(new Kategoria(reader));
                connection.Close();
            }
            return kategorie;
        }

        #endregion
    }
}