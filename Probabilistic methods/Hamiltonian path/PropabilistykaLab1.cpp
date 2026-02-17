// ConsoleApplication1.cpp : This file contains the 'main' function. Program execution begins and ends there.
//
#include <fstream>
#include <string>
#include <iostream>
using namespace std;
int suma = 1;
double wsp[15][17];
string Miasta[15];
string MiastaL[16];
int LudnoscMiasta[16];
int* min_przebieg = nullptr;
double min_dlug = 100000000.0f;
int* najb_przebieg = nullptr;
int najb_lud;
int suma_lud=0;
int polowa_lud;
void gen(int N, int* a, char* c, int zagl) {
    if (zagl == N - 1) {
        for (int i = 0; i < N; i++) {
            if (a[i] == 0) {
                c[zagl] = i + 1;
                cout << suma << " ";
                for (int j = 0; j < N; j++)
                    cout << (int)c[j] << " ";
                cout << endl;
                suma++;
                break;
            }
        }
    }
    else {
        for (int i = 0; i < N; i++) {
            if (a[i] == 0) {
                c[zagl] = i + 1;
                a[i] = 1;
                gen(N, a, c, zagl + 1);
                a[i] = 0;
            }
        }
    }
}

void gen_z_pow(int N, int* a, char* c, int zagl, int m, int pop_i) {
    if (zagl == N - 1) {
        for (int i = pop_i; i < m; i++) {
            c[zagl] = i + 1;
            cout << suma << " ";
            for (int j = 0; j < zagl + 1; j++)
                cout << (int)c[j] << " ";
            cout << endl;
            suma++;
        }
    }
    else {
        for (int i = pop_i; i < m; i++) {
            c[zagl] = i + 1;
            gen_z_pow(N, a, c, zagl + 1, m, i);
        }
    }

}

void czysc(int N, int* a) {
    for (int i = 0; i < N; i++) {
        a[i] = 0;
    }
}

void charczysz(int N, char* a) {
    for (int i = 0; i < N; i++) {
        a[i] = 0;
    }
}


void gen_do_miast(int pula_miast, int dlugosc_ciagu, int* odwiedzone,int* przebieg, int zagl, double* dlug, int pop) {
    if (zagl == dlugosc_ciagu - 1) {
        for (int i = 0; i < pula_miast; i++) {
            if (odwiedzone[i] == 0) {
                przebieg[zagl] = i;
                *dlug += wsp[pop][i + 2];
                *dlug += wsp[i][przebieg[0] + 2];
                if (*dlug < min_dlug) {
                    min_dlug = *dlug;
                    for (int k = 0; k < dlugosc_ciagu; k++) {
                        min_przebieg[k] = przebieg[k];
                    }
                }
                *dlug -= wsp[i][przebieg[0] + 2];
                *dlug -= wsp[pop][i + 2];
            }
        }
    }
    else {
        for (int i = 0; i < pula_miast; i++) {
            if (odwiedzone[i] == 0) {
                przebieg[zagl] = i;
                if (pop != -1) {
                    *dlug += wsp[pop][i + 2];
                }
                odwiedzone[i] = 1;
                gen_do_miast(pula_miast,dlugosc_ciagu,odwiedzone,przebieg,zagl+1,dlug,i);
                odwiedzone[i] = 0;
                if (pop != -1) {
                    *dlug -= wsp[pop][i + 2];
                }
                if (zagl == 0)
                    pop = -1;

            }
        }
    }
}

void gen_do_lud(int pula_miast, int dlugosc_ciagu, int* odwiedzone, int* przebieg, int zagl, int* lud) {
    if (zagl == dlugosc_ciagu - 1) {
        for (int i = 0; i < pula_miast; i++) {
            if (odwiedzone[i] == 0 || i == 0) {
                przebieg[zagl] = i;
                *lud += LudnoscMiasta[i];
                if (abs(polowa_lud-*lud) < abs(polowa_lud - najb_lud)) {
                    najb_lud = *lud;
                    for (int k = 0; k < dlugosc_ciagu; k++) {
                        najb_przebieg[k] = przebieg[k];
                    }
                }
                *lud -= LudnoscMiasta[i];
            }
        }
    }
    else {
        for (int i = 0; i < pula_miast; i++) {
            if (odwiedzone[i] == 0 || i==0) {
                przebieg[zagl] = i;
                *lud += LudnoscMiasta[i];
                odwiedzone[i] = 1;
                gen_do_lud(pula_miast, dlugosc_ciagu, odwiedzone, przebieg, zagl + 1, lud);
                odwiedzone[i] = 0;
                *lud -= LudnoscMiasta[i];
            }
        }
    }
}

void zad1() {
    int N = 5;
    int M = 7;
    int* a = new int[N];
    char* wypisz = new char[N];
    czysc(N, a);
    charczysz(N, wypisz);
    gen(N, a, wypisz, 0);
}

void zad2() {
    int N = 5;
    int M = 7;
    int* a = new int[N];
    int* prze = new int[N];
    char* wypisz = new char[N];
    czysc(N, a);
    charczysz(N, wypisz);
    gen_z_pow(N, a, wypisz, 0, 3, 0);
}

int zad3() {
    fstream plik;
    string s;

    plik.open("MPI lab 1 - italy.txt", std::ios::in);
    if (!plik.good()) {
        cout << "Blad otwierania" << endl;
        return 0;
    }

    for (int i = 0; i < 5; i++) //poczatek tabeli
        plik >> s;

    for (int i = 0; i < 15; i++) {
        plik >> s; // ind miasta
        plik >> Miasta[i]; // nazwa miasta
        plik >> s; // ludnosc
        plik >> wsp[i][0]; // x
        plik >> wsp[i][1]; // y
    }
    double o_x, o_y, sumofyx;
    int index;
    for (int i = 0; i < 15; i++) {
        index = 2;
        for (int j = 0; j < 15; j++) {
            o_x = wsp[i][0] - wsp[j][0];
            o_x = o_x * o_x;
            o_y = wsp[i][1] - wsp[j][1];
            o_y = o_y * o_y;
            sumofyx = o_x + o_y;
            wsp[i][index] = sqrt(sumofyx);
            index++;
        }
    }
    //od 2 do 16 odleglosci do roznych miast

    int dlug_ciagu = 15; //dlugosc ciagu
    int pula_miast = 15;//pula miast
    int* odwiedzone = new int[pula_miast];
    int* przebieg = new int[dlug_ciagu];
    min_przebieg = new int[dlug_ciagu];
    double dlug = 0.0f;
    czysc(pula_miast, odwiedzone);
    czysc(dlug_ciagu, min_przebieg);
    czysc(dlug_ciagu, przebieg);
    gen_do_miast(pula_miast, dlug_ciagu, odwiedzone, przebieg, 0, &dlug, -1);

    cout << min_dlug<<endl;
    for (int i = 0; i < dlug_ciagu; i++) {
        cout << Miasta[min_przebieg[i]]<<" ";
    }
    plik.close();
    return 1;
}

int zad4() {
    fstream plik;
    string s;

    plik.open("MPI lab 1 - italy.txt", std::ios::in);
    if (!plik.good()) {
        cout << "Blad otwierania" << endl;
        return 0;
    }

    for (int i = 0; i < 5; i++) //poczatek tabeli
        plik >> s;
    LudnoscMiasta[0] = 0;
    MiastaL[0] = "";
    for (int i = 0; i < 15; i++) {
        plik >> s; // ind miasta
        plik >> MiastaL[i+1]; // nazwa miasta
        plik >> LudnoscMiasta[i+1]; // ludnosc
        plik >> wsp[i][0]; // x
        plik >> wsp[i][1]; // y
    }
    int dlug_ciagu = 7;
    int pula_miast = 7 + 1;//7 mozliwych miast + 1 zerowe

    for (int i = 0; i < pula_miast; i++) {
        suma_lud += LudnoscMiasta[i];
    }


    polowa_lud = suma_lud/2;
    najb_lud = suma_lud;

    int* odwiedzone = new int[pula_miast];
    int* przebieg = new int[dlug_ciagu];
    najb_przebieg = new int[dlug_ciagu];
    int lud = 0;
    czysc(pula_miast, odwiedzone);
    czysc(dlug_ciagu, najb_przebieg);
    czysc(dlug_ciagu, przebieg);
    gen_do_lud(pula_miast, dlug_ciagu, odwiedzone, przebieg, 0, &lud);

    cout << "Polowa sumy " << polowa_lud << endl;
    cout <<"Najblizsza suma " << najb_lud << endl;
    for (int i = 0; i < dlug_ciagu; i++) {
        if(najb_przebieg[i]!=0)
            cout << MiastaL[najb_przebieg[i]] << " ";
    }
    plik.close();
    return 1;

}

int main()
{
    //zad1();
    //zad2();
    //zad3();
    zad4();
    return 0;
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
