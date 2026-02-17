Object subclass: #Wielokat
    instanceVariableNames: 'wierzcholki nazwa okrag_opi'
    classVariableNames: ''
    poolDictionaries: ''
    category: 'JezykiProgramowania'.

!Wielokat methodsFor: 'initialize-release'!
initialize: liczbaWierzcholkow name: nowaNazwa
    nazwa := nowaNazwa.
    wierzcholki := Array new: liczbaWierzcholkow.
    wierzcholki at: 1 put: 0@0.
! !

!Wielokat methodsFor: 'accessing'!
nazwa
	"podaje nazwe wielokata"

	^nazwa!

nazwa: nowa_nazwa
	"ustawia nowa nazwe wielokata"

	nazwa:=nowa_nazwa! !


!Wielokat methodsFor: 'actions'!
pole
    self subclassResponsibility
! !

!Wielokat methodsFor: 'actions'!
skaluj
    self subclassResponsibility
! !

!Wielokat methodsFor: 'actions'!
opiszOkrag
    self subclassResponsibility
! !

!Wielokat methodsFor: 'rendering'!
drukuj
    | pole |
    
    pole := self pole.

    Transcript show: nazwa printString; cr.
    Transcript show: 'Współrzędne wierzchołków:    '; cr.
    1 to: wierzcholki size do: [ :i |
        Transcript show: 'Wierzchołek ', i printString, ':    ', (wierzcholki at: i) printString; cr.
    ].
    Transcript show: 'Pole ', nazwa printString, ':    ', pole printString; cr.
! !


Wielokat subclass: #Kwadrat
    instanceVariableNames: ''
    classVariableNames: ''
    poolDictionaries: ''
    category: 'JezykiProgramowania'.

!Kwadrat methodsFor: 'arithmetic'!
+ figura
    | p |
    p := self pole + figura pole.
    ^ Kwadrat new initialize: p sqrt
! !

!Kwadrat methodsFor: 'actions'!
pole
    ^ (wierzcholki at: 2) x squared
! !

!Kwadrat methodsFor: 'actions'!
skaluj
    | skala bok_s |
    Transcript show: 'Podaj skale w postaci liczby zmiennoprzecinkowej:   '; flush.
    skala := (FileStream stdin nextLine) asNumber.
    bok_s := (wierzcholki at: 2) x * skala.
    wierzcholki at: 2 put: bok_s@0.
    wierzcholki at: 3 put: bok_s@bok_s.
    wierzcholki at: 4 put: 0@bok_s.
    okrag_opi setPromien: (((wierzcholki at: 2) x * (2 sqrt)) / 2).
    okrag_opi setSrodek: (bok_s / 2)@(bok_s / 2).
! !


!Kwadrat methodsFor: 'actions'!
opiszOkrag
    okrag_opi := Okrag new initialize: (((wierzcholki at: 2) x * (2 sqrt)) / 2) S: ((wierzcholki at: 2) x / 2)@((wierzcholki at: 2) x / 2).
! !


!Kwadrat methodsFor: 'rendering'!
drukujOkrag
    okrag_opi drukuj.
! !

!Kwadrat methodsFor: 'initialize-release'!
initialize: bok
    super initialize: 4 name: 'Kwadrat'.
    wierzcholki at: 2 put: bok@0.
    wierzcholki at: 3 put: bok@bok.
    wierzcholki at: 4 put: 0@bok.
! !

Wielokat subclass: #trojkat_rownoboczny
    instanceVariableNames: 'wysokosc'
    classVariableNames: ''
    poolDictionaries: ''
    category: 'JezykiProgramowania'.
    
!trojkat_rownoboczny methodsFor: 'actions'!
pole
    ^ (wierzcholki at: 2) x * wysokosc / 2
! !

!trojkat_rownoboczny methodsFor: 'arithmetic'!
+ figura
    | p |
    p := self pole + figura pole.
    p := (p / (3 sqrt)) * 4.
    ^ trojkat_rownoboczny new initialize: p sqrt
! !

!trojkat_rownoboczny methodsFor: 'actions'!
skaluj
    | skala bok_s |
    Transcript show: 'Podaj skale w postaci liczby zmiennoprzecinkowej:   '; flush.
    skala := (FileStream stdin nextLine) asNumber.
    bok_s := (wierzcholki at: 2) x * skala.
    wierzcholki at: 2 put: bok_s@0.0.
    wysokosc := (bok_s squared - (bok_s / 2) squared) sqrt.
    wierzcholki at: 3 put: (bok_s / 2)@wysokosc.
        okrag_opi setPromien:  ((wysokosc*2)/3).
    okrag_opi setSrodek: ((wierzcholki at: 2) x / 2)@(wysokosc/3).
! !

!trojkat_rownoboczny methodsFor: 'actions'!
opiszOkrag
    okrag_opi := Okrag new initialize: ((wysokosc*2)/3) S: ((wierzcholki at: 2) x / 2)@(wysokosc/3).
! !

!trojkat_rownoboczny methodsFor: 'rendering'!
drukujOkrag
    okrag_opi drukuj.
! !

!trojkat_rownoboczny methodsFor: 'initialize-release'!
initialize: bok 
    super initialize: 3 name: 'Trojkat rownoboczny'.
    wierzcholki at: 2 put: bok@0.0.
    wysokosc := (bok squared - (bok / 2) squared) sqrt.
    wierzcholki at: 3 put: (bok/2)@wysokosc.
! !


Object subclass: #Okrag
    instanceVariableNames: 'promien srodek nazwa'
    classVariableNames: ''
    poolDictionaries: ''
    category: 'JezykiProgramowania'.

!Okrag methodsFor: 'initialize-release'!
initialize: dlugoscPromienia S: srodekDef
    promien := dlugoscPromienia.
    srodek := srodekDef.
    nazwa := 'Okrag'.
! !

!Okrag methodsFor: 'actions'!
pole
    ^ Float pi * (promien squared).
! !

!Okrag methodsFor: 'accessing'!
setPromien: dlugoscPromienia
    promien := dlugoscPromienia.
! !

!Okrag methodsFor: 'accessing'!
setSrodek: srodekDef
    srodek := srodekDef.
! !

!Okrag methodsFor: 'actions'!
skaluj
    | skala |
    Transcript show: 'Podaj skale w postaci liczby zmiennoprzecinkowej: '; flush.
    skala := (FileStream stdin nextLine) asNumber.
    promien := promien * skala.
! !

!Okrag methodsFor: 'rendering'!
drukuj
    | polez |
    
    polez := self pole.
    
    Transcript show: nazwa printString; cr.
    Transcript show: 'Współrzędna środka: ', srodek printString; cr.
    Transcript show: 'Promien: ', promien printString; cr.
    Transcript show: 'Pole okregu: ', polez printString; cr.
! !

| trojkat1 trojkat2 suma |

trojkat1 := trojkat_rownoboczny new initialize: 3.0.
trojkat2 := trojkat_rownoboczny new initialize: 2.0.
trojkat1 drukuj.
trojkat2 drukuj.
result := trojkat1 + trojkat2.
result drukuj.
"^nowa figura"

trojkat1 opiszOkrag.
trojkat1 drukujOkrag.
trojkat1 skaluj.
trojkat1 drukuj.
trojkat1 drukujOkrag.
"^sklaowanie i okrag"

| kwadrat1 |

kwadrat1 := Kwadrat new initialize: 5.0.
kwadrat1 drukuj.
kwadrat1 opiszOkrag.
kwadrat1 drukujOkrag.
kwadrat1 skaluj.
kwadrat1 drukuj.
kwadrat1 drukujOkrag.
"^sklaowanie i okrag dla kwadratu"
