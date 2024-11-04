# Popis riesenia
Chcel som postupovat podla Clean Architecture, ale nakoniec som z toho upustil a nepokracoval som v tom. Dalej som neimplementoval Domain Driven Design.

Business logika je v Core, projekt LibraryCore. InfrastructureLayer obsahuje servisy, ako odosielanie emailu, alebo pristup do Databazy.
WebAPi obsahuje Rest API alebo webove projekty.
AutomaticProcesses obsahuje konzolove aplikacie ktore su spustane napriklad kazdu noc.
Testy su v adresari Tests.

# Testovanie
V testoch som mal testovat business loginu (Core), co dodrzujem. V UnitTestoch testujem funkcionalitu entity User. Tym ze som uplne neimplementoval Clean Architexcture, tak som to zmenil a v integracnych testoch testujem  BooksController.

Pre testovacie ucely je lepsie pouzit lokalnu databazu nastavenu podobne ako databaza na produkcnom serveri.
Niekedy sa daju pouzit produkcne data na testovacie ucely, ak su tie data spravne anonymizovane (zmena emailu, zmena tel cisla,...).

Validaciu a mozne error vstupy som uz netestoval, samozrejme pri normalnej aplikacii je to povinne.