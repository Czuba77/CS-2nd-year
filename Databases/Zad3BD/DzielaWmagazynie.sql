/*Wyœwietl jakie dzie³a znajdywa³y siê w magazynie x w okresie ( y - z )*/
SELECT 
DzielaSztuki.NumerDziela,
DzielaSztuki.AutorImie,
DzielaSztuki.AutorNazwisko,
 DzielaSztukiWPomieszczeniach.DataKoncowa,
 DzielaSztukiWPomieszczeniach.DataPoczatkowa
FROM DzielaSztuki inner join DzielaSztukiWPomieszczeniach on DzielaSztuki.NumerDziela=DzielaSztukiWPomieszczeniach.NumerDziela
WHERE DzielaSztukiWPomieszczeniach.NumerPomieszczenia = 5 AND DzielaSztukiWPomieszczeniach.DataKoncowa > '2023-03-05' AND DzielaSztukiWPomieszczeniach.DataPoczatkowa < '2023-03-03'
