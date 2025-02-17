﻿namespace WypozyczalaniaProjekt.Model
{
    using DAL.Encje;
    using DAL.Repozytoria;
    using System;
    using System.Collections.ObjectModel;
    using WypozyczalaniaProjekt.DAL;

    class Model
    {
        public ObservableCollection<Samochod> Samochody { get; set; } = new ObservableCollection<Samochod>();
        public ObservableCollection<Samochod> WyszukaneSamochody { get; set; } = new ObservableCollection<Samochod>();
        public ObservableCollection<Klient> Klienci { get; set; } = new ObservableCollection<Klient>();
        public ObservableCollection<Pracownik> Pracownicy { get; set; } = new ObservableCollection<Pracownik>();
        public ObservableCollection<Oddzial> Oddzialy { get; set; } = new ObservableCollection<Oddzial>();
        public ObservableCollection<Kategoria> Kategorie { get; set; } = new ObservableCollection<Kategoria>();
        public ObservableCollection<KartaKredytowa> Karty { get; set; } = new ObservableCollection<KartaKredytowa>();
        public ObservableCollection<Wynajem> Wynajmy { get; set; } = new ObservableCollection<Wynajem>();

        public int IdZalogowanegoPracownika { get; set; }

        private string nazwiskoZalogowanegoPracownika;
        public string NazwiskoZalogowanegoPracownika
        {
            get => nazwiskoZalogowanegoPracownika;
            set
            {
                nazwiskoZalogowanegoPracownika = value;
                foreach (var p in Pracownicy)
                {
                    if (p.Nazwisko == nazwiskoZalogowanegoPracownika)
                    {
                        IdZalogowanegoPracownika = (int)p.IdPracownik;
                    }
                }
            }
        }

        private string uzytkownik;
        public string Uzytkownik
        {
            get => uzytkownik;
            set
            {
                uzytkownik = value;
                if (uzytkownik == "Admin")
                {
                    database = DBConnectionAdmin.Instance;
                }
                else
                {
                    database = DBConnectionUser.Instance;
                }
            }
        }
        private static IDBConnection database;

        public Model()
        {
            database = DBConnectionUser.Instance;

            //pobranie danych z bazy do kolekcji
            var samochody = RepozytoriumSamochody.PobierzWszystkieSamochody(database);
            foreach (var s in samochody)
                Samochody.Add(s);

            var klienci = RepozytoriumKlienci.PobierzWszystkichKlientow(database);
            foreach (var k in klienci)
                Klienci.Add(k);

            var pracownicy = RepozytoriumPracownicy.PobierzWszystkichPracownikow(database);
            foreach (var p in pracownicy)
                Pracownicy.Add(p);

            var oddzialy = RepozytoriumOddzialy.PobierzWszystkieOddzialy(database);
            foreach (var o in oddzialy)
                Oddzialy.Add(o);

            var kategorie = RepozytoriumKategorie.PobierzWszystkieKategorie(database);
            foreach (var k in kategorie)
                Kategorie.Add(k);

            var karty = RepozytoriumKarty.PobierzWszystkieKarty(database);
            foreach (var k in karty)
                Karty.Add(k);

            var wynajmy = RepozytoriumWynajmy.PobierzWszystkieWynajmy(database);
            foreach (var w in wynajmy)
                Wynajmy.Add(w);
        }

        #region Samochód
        private bool CzySamochodJestJuzWBazie(Samochod samochod) => Samochody.Contains(samochod);

        public bool DodajSamochodDoBazy(Samochod samochod)
        {
            if (!CzySamochodJestJuzWBazie(samochod))
            {
                if (RepozytoriumSamochody.DodajSamochodDoBazy(database, samochod))
                {
                    Samochody.Add(samochod);
                    return true;
                }
            }
            return false;
        }

        public bool EdytujSamochodWBazie(Samochod samochod, sbyte idAuta)
        {
            if (RepozytoriumSamochody.EdytujSamochodWBazie(database, samochod, idAuta))
            {
                for (int i = 0; i < Samochody.Count; i++)
                {
                    if (Samochody[i].IdAuto == idAuta)
                    {
                        samochod.IdAuto = idAuta;
                        Samochody[i] = new Samochod(samochod);
                    }
                }
                return true;
            }
            return false;
        }

        public bool UsunSamochodZBazy(sbyte idAuta)
        {
            if (RepozytoriumSamochody.UsunSamochodZBazy(database, idAuta))
            {
                for (int i = 0; i < Samochody.Count; i++)
                {
                    if (Samochody[i].IdAuto == idAuta)
                    {
                        Samochody.RemoveAt(i);
                    }
                }
                return true;
            }
            return false;
        }

        public void SzukajSamochodow(DateTime r, DateTime z)
        {
            var samochody = RepozytoriumSamochody.PobierzWyszukaneSamochody(database, r, z);
            WyszukaneSamochody.Clear();
            foreach (var s in samochody)
                WyszukaneSamochody.Add(s);
        }

        public void PobierzWszystkieSamochody()
        {
            var samochody = RepozytoriumSamochody.PobierzWszystkieSamochody(database);
            Samochody.Clear();
            foreach (var s in samochody)
                Samochody.Add(s);
        }

        #endregion

        #region Pracownik
        private bool CzyPracownikJestJuzWBazie(Pracownik pracownik) => Pracownicy.Contains(pracownik);

        public bool DodajPracownikaDoBazy(Pracownik pracownik)
        {
            if (!CzyPracownikJestJuzWBazie(pracownik))
            {
                if (RepozytoriumPracownicy.DodajPracownikaDoBazy(database, pracownik))
                {
                    Pracownicy.Add(pracownik);
                    return true;
                }
            }
            return false;
        }

        public bool EdytujPracownikaWBazie(Pracownik pracownik, sbyte idPracownik)
        {
            if (RepozytoriumPracownicy.EdytujPracownikaWBazie(database, pracownik, idPracownik))
            {
                for (int i = 0; i < Pracownicy.Count; i++)
                {
                    if (Pracownicy[i].IdPracownik == idPracownik)
                    {
                        pracownik.IdPracownik = idPracownik;
                        Pracownicy[i] = new Pracownik(pracownik);
                    }
                }
                return true;
            }
            return false;
        }

        public bool UsunPracownikaZBazy(sbyte idPracownik)
        {
            if (RepozytoriumPracownicy.UsunPracownikaZBazy(database, idPracownik))
            {
                for (int i = 0; i < Pracownicy.Count; i++)
                {
                    if (Pracownicy[i].IdPracownik == idPracownik)
                    {
                        Pracownicy.RemoveAt(i);
                    }
                }
                return true;
            }
            return false;
        }

        #endregion

        #region Klient
        private bool CzyKlientJestJuzWBazie(Klient klient) => Klienci.Contains(klient);

        public bool DodajKlientaDoBazy(Klient klient)
        {
            if (!CzyKlientJestJuzWBazie(klient))
            {
                if (RepozytoriumKlienci.DodajKlientaDoBazy(database, klient))
                {
                    Klienci.Add(klient);
                    return true;
                }
            }
            return false;
        }

        public bool EdytujKlientaWBazie(Klient klient, sbyte idKlient)
        {
            if (RepozytoriumKlienci.EdytujKlientaWBazie(database, klient, idKlient))
            {
                for (int i = 0; i < Klienci.Count; i++)
                {
                    if (Klienci[i].IdKlient == idKlient)
                    {
                        klient.IdKlient = idKlient;
                        Klienci[i] = new Klient(klient);
                    }
                }
                return true;
            }
            return false;
        }

        public bool UsunKlientaZBazy(sbyte idKlient)
        {
            if (RepozytoriumKlienci.UsunKlientaZBazy(database, idKlient))
            {
                for (int i = 0; i < Klienci.Count; i++)
                {
                    if (Klienci[i].IdKlient == idKlient)
                    {
                        Klienci.RemoveAt(i);
                    }
                }
                return true;
            }
            return false;
        }

        #endregion

        #region Oddział
        private bool CzyOddzialJestJuzWBazie(Oddzial oddzial) => Oddzialy.Contains(oddzial);

        public bool DodajOddzialDoBazy(Oddzial oddzial)
        {
            if (!CzyOddzialJestJuzWBazie(oddzial))
            {
                if (RepozytoriumOddzialy.DodajOddzialDoBazy(database, oddzial))
                {
                    Oddzialy.Add(oddzial);
                    return true;
                }
            }
            return false;
        }

        public bool EdytujOddzialWBazie(Oddzial oddzial, sbyte idOddzialu)
        {
            if (RepozytoriumOddzialy.EdytujOddzialWBazie(database, oddzial, idOddzialu))
            {
                for (int i = 0; i < Oddzialy.Count; i++)
                {
                    if (Oddzialy[i].IdOddzialu == idOddzialu)
                    {
                        oddzial.IdOddzialu = idOddzialu;
                        Oddzialy[i] = new Oddzial(oddzial);
                    }
                }
                return true;
            }
            return false;
        }

        public bool UsunOddzialZBazy(sbyte idOddzialu)
        {
            if (RepozytoriumOddzialy.UsunOddzialZBazy(database, idOddzialu))
            {
                for (int i = 0; i < Oddzialy.Count; i++)
                {
                    if (Oddzialy[i].IdOddzialu == idOddzialu)
                    {
                        Oddzialy.RemoveAt(i);
                    }
                }
                return true;
            }
            return false;
        }

        #endregion

        #region Karta
        private bool CzyKartaJestJuzWBazie(KartaKredytowa karta) => Karty.Contains(karta);

        public bool DodajKarteDoBazy(KartaKredytowa karta)
        {
            if (!CzyKartaJestJuzWBazie(karta))
            {
                if (RepozytoriumKarty.DodajKarteDoBazy(database, karta))
                {
                    Karty.Add(karta);
                    return true;
                }
            }
            return false;
        }

        public bool EdytujKarteWBazie(KartaKredytowa karta, sbyte idKarty, Klient klient)
        {
            if (RepozytoriumKarty.EdytujKarteWBazie(database, karta, idKarty, klient))
            {
                for (int i = 0; i < Karty.Count; i++)
                {
                    if (Karty[i].IdKarty == idKarty)
                    {
                        karta.IdKarty = idKarty;
                        Karty[i] = new KartaKredytowa(karta);
                    }
                }
                return true;
            }
            return false;
        }

        public bool UsunKarteZBazy(sbyte idKarty)
        {
            if (RepozytoriumKarty.UsunKarteZBazy(database, idKarty))
            {
                for (int i = 0; i < Karty.Count; i++)
                {
                    if (Karty[i].IdKarty == idKarty)
                    {
                        Karty.RemoveAt(i);
                    }
                }
                return true;
            }
            return false;
        }

        #endregion

        #region Wynajem
        private bool CzyWynajemJestJuzWBazie(Wynajem wynajem) => Wynajmy.Contains(wynajem);

        public bool DodajWynajemDoBazy(Wynajem wynajem)
        {
            if (!CzyWynajemJestJuzWBazie(wynajem))
            {
                if (RepozytoriumWynajmy.DodajWynajemDoBazy(database, wynajem))
                {
                    Wynajmy.Add(wynajem);
                    return true;
                }
            }
            return false;
        }

        public bool EdytujWynajemWBazie(Wynajem wynajem, sbyte idWynajem)
        {
            if (RepozytoriumWynajmy.EdytujWynajemWBazie(database, wynajem, idWynajem))
            {
                for (int i = 0; i < Wynajmy.Count; i++)
                {
                    if (Wynajmy[i].IdWynajem == idWynajem)
                    {
                        wynajem.IdWynajem = idWynajem;
                        Wynajmy[i] = new Wynajem(wynajem);
                    }
                }
                return true;
            }
            return false;
        }

        public bool UsunWynajemZBazy(sbyte idWynajem)
        {
            if (RepozytoriumWynajmy.UsunWynajemZBazy(database, idWynajem))
            {
                for (int i = 0; i < Wynajmy.Count; i++)
                {
                    if (Wynajmy[i].IdWynajem == idWynajem)
                    {
                        Wynajmy.RemoveAt(i);
                    }
                }
                return true;
            }
            return false;
        }

        #endregion
    }
}
