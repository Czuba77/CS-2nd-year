/*Wyœwietl zestawienie magazynów w których temperatura wynosi wiecej niz 18 stopni ale mniej ni¿ 22, oraz wilgotnoœæ wynosi wiecej niz 45%, oraz mniej ni¿ 60% */
SELECT Magazyny.NumerPomieszczenia, Magazyny.Wilgotnosc, Magazyny.Temperatura  
FROM Magazyny
WHERE  (Wilgotnosc BETWEEN 45.0 AND 65.0) AND (Temperatura BETWEEN 18.0 AND 24.0)
GROUP BY NumerPomieszczenia,Wilgotnosc,  Temperatura
ORDER BY 1 ASC