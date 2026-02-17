/*Policz ile obrazow znajduje sie w tej chwili w danych salach wystawowych i odejmij ta liczbe od ilosci_miejsc_na_obrazy i posortuj malej¹co po tej roznice*/
SELECT
SaleWystawowe.NumerPomieszczenia,
IloscMiejscNaObrazy-IleObrazow as IleWolnychMiejscNaObrazy
FROM
(SELECT 
DzielaSztukiWPomieszczeniach.NumerPomieszczenia,
count (*) AS IleObrazow
FROM
DzielaSztukiWPomieszczeniach inner join Obrazy on Obrazy.NumerDziela=DzielaSztukiWPomieszczeniach.NumerDziela
WHERE DzielaSztukiWPomieszczeniach.DataKoncowa is null
GROUP BY DzielaSztukiWPomieszczeniach.NumerPomieszczenia) as A inner join SaleWystawowe on SaleWystawowe.NumerPomieszczenia = A.NumerPomieszczenia
ORDER BY 2 DESC