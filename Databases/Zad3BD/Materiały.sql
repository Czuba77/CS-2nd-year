/*Zestaw obrazy ka¿dego artysty które znajduj¹ siê na terenie Galerii i wyœwietl jaki rodzaj farby wybiera³ najczêœciej, posortuj po nazwisku*/
SELECT A.AutorImie, A.AutorNazwisko,A.Ilosc_Dziel, RodzajFarby
FROM
(SELECT AutorImie, AutorNazwisko, max(IloscDziel) as Ilosc_Dziel
FROM
(SELECT AutorImie,AutorNazwisko,RodzajFarby,
COUNT (*) AS IloscDziel
FROM DzielaSztuki inner join Obrazy on DzielaSztuki.NumerDziela=Obrazy.NumerDziela
GROUP BY RodzajFarby,AutorImie,AutorNazwisko) 
AS Y
GROUP BY AutorImie,AutorNazwisko) 
AS A
inner join (SELECT AutorImie,AutorNazwisko,RodzajFarby,
COUNT (*) AS IloscDziel
FROM DzielaSztuki inner join Obrazy on DzielaSztuki.NumerDziela=Obrazy.NumerDziela
GROUP BY RodzajFarby,AutorImie,AutorNazwisko) 
AS B ON A.AutorNazwisko = B.AutorNazwisko AND A.Ilosc_Dziel = B.IloscDziel
ORDER BY 2 ASC
