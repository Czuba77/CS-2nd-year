/*Policz ilosc biletow kupionych na dane prelekcje podziel je przez ilosc miejsc i wyswietl prelekcje sortujac je  malejaco po tym stosunku*/
SELECT Tytul, cast(count(NumerBiletu) AS FLOAT)/IloscMiejsc AS ilosc_kupionych FROM PrelekcjeBiletyStosunek
GROUP BY IloscMiejsc,NumerWydarzenia,  Tytul
ORDER BY 2 DESC