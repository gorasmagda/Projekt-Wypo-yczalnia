﻿using System;

namespace WypozyczalaniaProjekt.ViewModel
{
    using BaseClassess;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using WypozyczalaniaProjekt.DAL.Encje;
    using WypozyczalaniaProjekt.Model;
    class PracownicyViewModel : ViewModelBase
    {

        #region Składowe prywatne

        private Model model = null;
        private Pracownik wybranyPracownik;

        private int idWybranegoPracownika;
        private int? idOddzial;
        private decimal? pensja;
        private string imie, nazwisko, plec, nrTelefonu, dataUrodzenia, adres, email, nrPrawaJazdy, pesel;

        #endregion

        #region Konstruktory

        public PracownicyViewModel(Model model)
        {
            Pracownicy = new ObservableCollection<Pracownik>();
            this.model = model;
            Pracownicy = model.Pracownicy;
            IdWybranegoPracownika = -1;
        }

        #endregion

        #region Właściwości

        public ObservableCollection<Pracownik> Pracownicy { get; set; }

        public Pracownik WybranyPracownik
        {
            get => wybranyPracownik;
            set
            {
                wybranyPracownik = value;
                onPropertyChanged(nameof(WybranyPracownik));
                if (wybranyPracownik != null)
                    ZaladujFormularz();
            }
        }

        public int IdWybranegoPracownika
        {
            get => idWybranegoPracownika;
            set
            {
                idWybranegoPracownika = value;
                onPropertyChanged(nameof(IdWybranegoPracownika));
            }
        }


        public string Imie
        {
            get => imie;
            set
            {
                imie = value;
                onPropertyChanged(nameof(Imie));
            }
        }

        public string Plec
        {
            get => plec;
            set
            {
                plec = value;
                onPropertyChanged(nameof(Plec));
            }
        }

        public string NrTelefonu
        {
            get => nrTelefonu;
            set
            {
                nrTelefonu = value;
                onPropertyChanged(nameof(NrTelefonu));
            }
        }

        public string DataUrodzenia
        {
            get => dataUrodzenia;
            set
            {
                dataUrodzenia = value;
                onPropertyChanged(nameof(DataUrodzenia));
            }
        }

        public decimal? Pensja
        {
            get => pensja;
            set
            {
                pensja = value;
                onPropertyChanged(nameof(Pensja));
            }
        }

        public int? IdOddzial
        {
            get => idOddzial;
            set
            {
                idOddzial = value;
                onPropertyChanged(nameof(IdOddzial));
            }
        }

        public string Pesel
        {
            get => pesel;
            set
            {
                pesel = value;
                onPropertyChanged(nameof(Pesel));
            }
        }

        public string Nazwisko
        {
            get => nazwisko;
            set
            {
                nazwisko = value;
                onPropertyChanged(nameof(Nazwisko));
            }
        }

        public string Adres
        {
            get => adres;
            set
            {
                adres = value;
                onPropertyChanged(nameof(Adres));
            }
        }

        public string Email
        {
            get => email;
            set
            {
                email = value;
                onPropertyChanged(nameof(Email));
            }
        }

        public string NrPrawaJazdy
        {
            get => nrPrawaJazdy;
            set
            {
                nrPrawaJazdy = value;
                onPropertyChanged(nameof(NrPrawaJazdy));
            }
        }

        #endregion

        #region Polecenia

        private ICommand dodajPracownika = null;
        public ICommand DodajPracownika
        {
            get
            {
                if (dodajPracownika == null)
                    dodajPracownika = new RelayCommand(
                        arg =>
                        {
                            var pracownik = new Pracownik(Imie, Nazwisko, Plec, Email,NrTelefonu, Adres, Pesel, NrPrawaJazdy, DateTime.Parse(DataUrodzenia), (sbyte)IdOddzial, (decimal)Pensja);
                            if (model.DodajPracownikaDoBazy(pracownik))
                            {
                                CzyscFormularz();
                                System.Windows.MessageBox.Show("Pracownik został dodany!");
                            }
                        },
                        null);
                return dodajPracownika;
            }
        }

        private ICommand edytujPracownika = null;
        public ICommand EdytujPracownika
        {
            get
            {
                if (edytujPracownika == null)
                    edytujPracownika = new RelayCommand(
                        arg =>
                        {
                            // TODO: PracownicyVM - Edycja Pracownika
                        },
                        null);
                return edytujPracownika;
            }
        }

        private ICommand usunPracownika = null;

        public ICommand UsunPracownika
        {
            get
            {
                if (usunPracownika == null)
                    usunPracownika = new RelayCommand(
                        arg =>
                        {
                            // TODO: PracownicyVM - Usuwanie Pracownika
                        },
                        null);
                return usunPracownika;
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

        #endregion

        private void ZaladujFormularz()
        {
            if (IdWybranegoPracownika > -1)
            {
                Imie = WybranyPracownik.Imie;
                Nazwisko = WybranyPracownik.Nazwisko;
                Plec = WybranyPracownik.Plec;
                NrTelefonu = WybranyPracownik.NrTelefonu;
                DataUrodzenia = WybranyPracownik.DataUrodzenia.ToString("yyyy-MM-dd");
                Adres = WybranyPracownik.Adres;
                Email = WybranyPracownik.Email;
                NrPrawaJazdy = WybranyPracownik.NrPrawaJazdy;
                Pesel = WybranyPracownik.Pesel;
                IdOddzial = (int)WybranyPracownik.IdOddzial;
                Pensja = wybranyPracownik.Pensja;
            }
            else
            {
                CzyscFormularz();
            }
            
        }
        private void CzyscFormularz()
        {
            Imie = "";
            Nazwisko = "";
            Plec = "";
            NrTelefonu = "";
            DataUrodzenia = "";
            Adres = "";
            Email = "";
            NrPrawaJazdy = "";
            Pesel = "";
            IdOddzial = null;
            Pensja = null;
        }
    }
}
