/*Policz ilosc biletow kupionych na dane prelekcje podziel je przez ilosc miejsc i wyswietl prelekcje sortujac je  malejaco po tym stosunku*/
CREATE VIEW PrelekcjeBiletyStosunek AS 
SELECT 
Prelekcje.IloscMiejsc, 
Prelekcje.NumerWydarzenia,
WydarzeniaKulturalne.Tytul, 
BiletyZakupione.NumerBiletu
FROM
(Prelekcje inner join WydarzeniaKulturalne on 
Prelekcje.NumerWydarzenia=WydarzeniaKulturalne.NumerWydarzenia)
inner join BiletyZakupione on 
BiletyZakupione.NumerWydarzenia=WydarzeniaKulturalne.NumerWydarzenia



