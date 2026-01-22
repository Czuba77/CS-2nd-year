/*Wyœwietl zestawienie emaili wszystkich klientów posiadaj¹cych bilet na dane wydarzenie*/
SELECT 
Klienci.ID,
Klienci.Imie,
Klienci.Nazwisko,
Klienci.NumerTelefonu,
AdresMailowy,
BiletyZakupione.NumerWydarzenia
FROM
Klienci inner join BiletyZakupione on 
Klienci.ID=BiletyZakupione.KlientID
WHERE BiletyZakupione.NumerWydarzenia=3
