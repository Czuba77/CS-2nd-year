/*Wyswietl wszystkie rzeŸby które potrzebuj¹ konserwacji sortuj¹c je po dacie DataKolejnejKonserwacji rosn¹co*/

SELECT 
DzielaSztuki.NumerDziela,
DzielaSztuki.Tytul,
DzielaSztuki.TerminKolejnejKonserwacji
FROM 
DzielaSztuki inner join Rzezby on DzielaSztuki.NumerDziela=Rzezby.NumerDziela
WHERE DzielaSztuki.TerminKolejnejKonserwacji < GETDATE()
ORDER BY 3 ASC