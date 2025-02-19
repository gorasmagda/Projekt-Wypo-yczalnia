﻿using System;

namespace WypozyczalaniaProjekt.ViewModel
{
    using BaseClassess;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;
    using WypozyczalaniaProjekt.DAL.Encje;
    using WypozyczalaniaProjekt.Model;
    class KlienciViewModel : ViewModelBase
    {

        #region Składowe prywatne

        private Model model = null;

        private int idWybranegoKlienta;
        private int? idKarty;
        private string imie, nazwisko, plec, dataUrodzenia, pesel, nrTelefonu, adres, email, nrPrawaJazdy;

        private string numer, dataWaznosci, numerCVV, rodzaj;

        #endregion

        #region Konstruktory

        public KlienciViewModel(Model model)
        {
            Klienci = new ObservableCollection<Klient>();
            Plcie = new List<string>();
            Rodzaje = new List<string>();
            Plcie.Add("kobieta");
            Plcie.Add("mężczyzna");
            Rodzaje.Add("kredytowa");
            Rodzaje.Add("debetowa");
            Rodzaje.Add("obciążeniowa");
            Rodzaje.Add("przedpłacona");
            Rodzaje.Add("wirtualna");

            this.model = model;
            Klienci = model.Klienci;
            idWybranegoKlienta = -1;
        }

        #endregion

        #region Właściwości

        public ObservableCollection<Klient> Klienci { get; set; }
        public List<string> Plcie { get; set; }
        public List<string> Rodzaje { get; set; }

        private Klient wybranyKlient;
        public Klient WybranyKlient
        {
            get => wybranyKlient;
            set
            {
                wybranyKlient = value;
                if (wybranyKlient != null)
                {
                    DeleteEnabled = true;
                }
                else
                {
                    DeleteEnabled = false;
                }
                onPropertyChanged(nameof(WybranyKlient));
            }
        }

        public int IdWybranegoKlienta
        {
            get => idWybranegoKlienta;
            set
            {
                idWybranegoKlienta = value;
                SprawdzFormularz();
                onPropertyChanged(nameof(IdWybranegoKlienta));
            }
        }


        public string Imie
        {
            get => imie;
            set
            {
                imie = value;
                SprawdzFormularz();
                onPropertyChanged(nameof(Imie));
            }
        }
        public string Plec
        {
            get => plec;
            set
            {
                plec = value;
                SprawdzFormularz();
                onPropertyChanged(nameof(Plec));
            }
        }
        public string NrTelefonu
        {
            get => nrTelefonu;
            set
            {
                nrTelefonu = value;
                SprawdzFormularz();
                onPropertyChanged(nameof(NrTelefonu));
            }
        }
        public string DataUrodzenia
        {
            get => dataUrodzenia;
            set
            {
                dataUrodzenia = value;
                SprawdzFormularz();
                onPropertyChanged(nameof(DataUrodzenia));
            }
        }
        public string Pesel
        {
            get => pesel;
            set
            {
                pesel = value;
                SprawdzFormularz();
                onPropertyChanged(nameof(Pesel));
            }
        }
        public string Nazwisko
        {
            get => nazwisko;
            set
            {
                nazwisko = value;
                SprawdzFormularz();
                onPropertyChanged(nameof(Nazwisko));
            }
        }
        public string Adres
        {
            get => adres;
            set
            {
                adres = value;
                SprawdzFormularz();
                onPropertyChanged(nameof(Adres));
            }
        }
        public string Email
        {
            get => email;
            set
            {
                email = value;
                SprawdzFormularz();
                onPropertyChanged(nameof(Email));
            }
        }
        public string NrPrawaJazdy
        {
            get => nrPrawaJazdy;
            set
            {
                nrPrawaJazdy = value;
                SprawdzFormularz();
                onPropertyChanged(nameof(NrPrawaJazdy));
            }
        }
        public int? IdKarty
        {
            get => idKarty;
            set
            {
                idKarty = value;
                SprawdzFormularz();
                onPropertyChanged(nameof(IdKarty));
            }
        }

        public string Numer
        {
            get => numer;
            set
            {
                numer = value;
                SprawdzFormularz();
                onPropertyChanged(nameof(Numer));
            }
        }
        public string DataWaznosci
        {
            get => dataWaznosci;
            set
            {
                dataWaznosci = value;
                SprawdzFormularz();
                onPropertyChanged(nameof(DataWaznosci));
            }
        }
        public string NumerCVV
        {
            get => numerCVV;
            set
            {
                numerCVV = value;
                SprawdzFormularz();
                onPropertyChanged(nameof(NumerCVV));
            }
        }
        public string Rodzaj
        {
            get => rodzaj;
            set
            {
                rodzaj = value;
                SprawdzFormularz();
                onPropertyChanged(nameof(Rodzaj));
            }
        }

        #endregion

        #region Polecenia

        private ICommand dodajKlienta = null;
        public ICommand DodajKlienta
        {
            get
            {
                if (dodajKlienta == null)
                    dodajKlienta = new RelayCommand(
                        arg =>
                        {
                            var karta = new KartaKredytowa(Numer, DateTime.Parse(DataWaznosci), NumerCVV, Imie, Nazwisko, Rodzaj);
                            if (model.DodajKarteDoBazy(karta))
                            {
                                var noweIdKarty = model.Karty[model.Karty.Count - 1].IdKarty;
                                var klient = new Klient(Imie, Nazwisko, Plec, Email, NrTelefonu, Adres, Pesel, NrPrawaJazdy, DateTime.Parse(DataUrodzenia), (sbyte)noweIdKarty);
                                if (model.DodajKlientaDoBazy(klient))
                                {
                                    CzyscFormularz();
                                    MessageBox.Show("Klient został dodany!");
                                }
                            }
                        },
                        arg => SprawdzFormularz());
                return dodajKlienta;
            }
        }

        private ICommand edytujKlienta = null;
        public ICommand EdytujKlienta
        {
            get
            {
                if (edytujKlienta == null)
                    edytujKlienta = new RelayCommand(
                        arg =>
                        {

                            var tempDataWaznosci = DataWaznosci;
                            var tempWybranyKlient = WybranyKlient;
                            var tempNumer = Numer;
                            var tempNumerCVV = NumerCVV;
                            var tempRodzaj = Rodzaj;
                            if (model.EdytujKlientaWBazie(new Klient(Imie, Nazwisko, Plec, Email, NrTelefonu, Adres, Pesel, NrPrawaJazdy, DateTime.Parse(DataUrodzenia), (sbyte)IdKarty), (sbyte)WybranyKlient.IdKlient))
                            {
                                if (model.EdytujKarteWBazie(new KartaKredytowa(tempNumer, DateTime.Parse(tempDataWaznosci), tempNumerCVV, Imie, Nazwisko, tempRodzaj), (sbyte)tempWybranyKlient.IdKarty, tempWybranyKlient))
                                {
                                    CzyscFormularz();
                                    IdWybranegoKlienta = -1;
                                    MessageBox.Show("Edycja klienta oraz karty przebiegła pomyślnie");
                                }
                            }
                        },
                        arg => IdWybranegoKlienta > -1);
                return edytujKlienta;
            }
        }

        private ICommand usunKlienta = null;
        public ICommand UsunKlienta
        {
            get
            {
                if (usunKlienta == null)
                    usunKlienta = new RelayCommand(
                        arg =>
                        {
                            if (MessageBox.Show("Czy chcesz usunąć wybranego klienta?", "Usuwanie klienta", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                            {
                                var tempWybranyKlient = WybranyKlient;
                                if (model.UsunKlientaZBazy((sbyte)WybranyKlient.IdKlient))
                                {
                                    if (model.UsunKarteZBazy((sbyte)tempWybranyKlient.IdKarty))
                                    {
                                        CzyscFormularz();
                                        IdWybranegoKlienta = -1;
                                        MessageBox.Show("Usunięto wybranego klienta wraz z jego kartą");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Usuwanie nie powiodło się");
                                }
                            }
                        },
                        arg => IdWybranegoKlienta > -1);
                return usunKlienta;
            }
        }

        private ICommand wyczysc = null;
        public ICommand Wyczysc
        {
            get
            {
                if (wyczysc == null)
                    wyczysc = new RelayCommand(
                        arg =>
                        {
                            CzyscFormularz();
                        },
                        null);
                return wyczysc;
            }
        }


        private ICommand zaladujFormularz = null;
        public ICommand ZaladujFormularz
        {
            get
            {
                if (zaladujFormularz == null)
                    zaladujFormularz = new RelayCommand(
                        o =>
                        {
                            if (IdWybranegoKlienta > -1)
                            {
                                Imie = WybranyKlient.Imie;
                                Nazwisko = WybranyKlient.Nazwisko;
                                Plec = WybranyKlient.Plec;
                                DataUrodzenia = WybranyKlient.DataUrodzenia.ToString("yyyy-MM-dd");
                                Pesel = WybranyKlient.Pesel;
                                NrTelefonu = WybranyKlient.NrTelefonu;
                                Adres = WybranyKlient.Adres;
                                Email = WybranyKlient.Email;
                                NrPrawaJazdy = WybranyKlient.NrPrawaJazdy;
                                IdKarty = (int)WybranyKlient.IdKarty;
                                foreach (var k in model.Karty)
                                {
                                    if (WybranyKlient.IdKarty == k.IdKarty)
                                    {
                                        Rodzaj = k.Rodzaj;
                                        DataWaznosci = k.DataWaznosci.ToString("yyyy-MM-dd");
                                        Numer = k.Numer;
                                        NumerCVV = k.NumerCVV;
                                    }
                                }
                            }
                            else
                            {
                                CzyscFormularz();
                            }
                        },
                        null);
                return zaladujFormularz;
            }
        }


        #endregion

        #region Wyłączanie przycisków

        private bool addEnabled, editEnabled, deleteEnabled;
        public bool AddEnabled
        {
            get => addEnabled;
            set
            {
                addEnabled = value;
                onPropertyChanged(nameof(AddEnabled));
            }
        }

        public bool EditEnabled
        {
            get => editEnabled;
            set
            {
                editEnabled = value;
                onPropertyChanged(nameof(EditEnabled));
            }
        }

        public bool DeleteEnabled
        {
            get => deleteEnabled;
            set
            {
                deleteEnabled = value;
                onPropertyChanged(nameof(DeleteEnabled));
            }
        }

        #endregion

        private void CzyscFormularz()
        {
            Imie = "";
            Nazwisko = "";
            Plec = "";
            DataUrodzenia = "";
            Pesel = "";
            NrTelefonu = "";
            Adres = "";
            Email = "";
            NrPrawaJazdy = "";
            IdKarty = null;
            WybranyKlient = null;

            Numer = "";
            DataWaznosci = "";
            NumerCVV = "";
            Rodzaj = "";

        }

        private bool SprawdzFormularz()
        {
            bool wynik = true;

            if (IdKarty == null || Numer == null || DataWaznosci == null || NumerCVV == null || Rodzaj == null)
                wynik = false;
            if (Imie == "" || Nazwisko == "" || Plec == "" || DataUrodzenia == "" ||
                Pesel == "" || NrTelefonu == "" || Adres == "" || Email == "" || NrPrawaJazdy == "" ||
                Numer == "" || DataWaznosci == "" || NumerCVV == "" || Rodzaj == "")
                wynik = false;

            AddEnabled = wynik;
            EditEnabled = wynik;
            return wynik;
        }
    }
}
