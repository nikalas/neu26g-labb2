# Labb 2 - Dungeon Crawler
En dungeon crawler är en typ av rollspel som involverar att spelare utforskar labyrintiska områden, så kallade dungeons, där de slåss mot fiender och letar efter skatter. I denna labb bygger vi en, något förenklad, version av ett sådant spel i form av en konsolapplikation.

Spelgenren roguelike bygger oftast på så kallad procedural generation, som är en metod för ta fram slumpmässiga banor med hjälp av algoritmer. Eftersom fokus på denna labb ska vara objektorienterad programmering, så har jag valt bort den delen och istället skapat en färdig bana som ni får i form av en textfil. (Ladda ner Level1.txt, längst ner på sidan.)

Filen representerar en “dungeon” med två olika sorters monster (“rats” & “snakes”) utplacerade, och har även en fördefinierad startposition för spelaren. Din uppgift blir att skriva kod som läser in filen och delar in i olika objekt (väggar, spelare och fiender) i C# som fristående från varandra håller reda på sina egna data (t.ex. position, färg, hälsa) och metoder (t.ex för att förflytta sig, eller attackera).


## Klasshierarki av Level Elements
Förutom själva spelaren finns det 3 olika sorters objekt i vår “dungeon”: “Wall”, “Rat”, och “Snake”. Vi vill använda arv för att återanvända så mycket kod som möjligt för den funktionalitet som delas av flera typer av objekt.

Det ska finns en abstrakt basklass som jag valt att kalla “LevelElement”. Eftersom den är abstrakt så kan man inte ha instanser av denna, utan den används för att definiera basfunktionalitet som andra klasser sedan kan ärva. LevelElement ska ha properties för (X,Y)-position, en char som lagrar vilket tecken en klass ritas ut med (t.ex. kommer “Wall” använda #-tecknet), samt en ConsoleColor som lagrar vilken färg tecknet ska ritas med. Den ska dessutom ha en publik Draw-metod (utan parametrar), som vi kan anropa för att rita ut ett LevelElement med rätt färg och tecken på rätt plats.

Klassen **“Wall”** ärver av LevelElement, och behöver egentligen ingen egen kod förutom att hårdkoda färgen och tecknet för vägg (en grå hashtag).

Klassen **“Enemy**” ärver också av LevelElement, och lägger till funktionalitet som är specifik för fiender. Även Enemy är abstrakt, då vi inte vill att man ska kunna instansiera ospecifika “fiender”. Alla riktiga fiender (i labben rat & snake, men om man vill och har tid får man lägga till fler typer av fiender) ärver av denna klass. Enemy ska ha properties för namn (t.ex snake/rat), hälsa (HP), samt AttackDice och DefenceDice av typen Dice (mer om detta längre ner). Den ska även ha en abstrakt Update-metod, som alltså inte implementeras i denna klass, men som kräver att alla som ärver av klassen implementerar den. Vi vill alltså kunna anropa Update-metoden på alla fiender och sedan sköter de olika subklasserna hur de uppdateras (till exempel olika förflyttningsmönster).

Slutligen har vi klasserna **“Rat”** och **“Snake”** som initialiserar sina nedärvda properties med de unika egenskaper som respektive fiende har, samt även implementerar Update-metoden på sina egna unika sätt.


## Läsa in bandesign från fil
Skapa en klass “LevelData” som har en private field “elements” av typ List<LevelElement>. Denna ska även exponeras i form av en public readonly property “Elements”.

Vidare har LevelData en metod, Load(string filename), som läser in data från filen man anger vid anrop. Load läser igenom textfilen tecken för tecken, och för varje tecken den hittar (som är någon av #, r, eller s) så skapar den en ny instans av den klass som motsvarar tecknet och lägger till en referens till instansen på “elements”-listan. Tänk på att när instansen skapas så måste den även få en (X/Y) position; d.v.s Load behöver alltså hålla reda på vilken rad och kolumn i filen som tecknet hittades på. Den behöver även spara undan startpositionen för spelaren när den stöter på @.

När filen är inläst bör det alltså finnas ett objekt i “elements” för varje tecken i filen (exkluderat space/radbyte), och en enkel foreach-loop som anropar .Draw() för varje element i listan bör nu rita upp hela banan på skärmen.


## Game Loop
En game loop är en loop som körs om och om igen medan spelet är igång, och i vårat fall kommer ett varv i loopen motsvaras av en omgång i spelet. För varje varv i loopen inväntar vi att användaren trycker in en knapp; sedan utför vi spelarens drag, följt av datorns drag (uppdatera alla fiender), innan vi loopar igen. Möjligtvis kan man ha en knapp (Esc) för att avsluta loopen/spelet.

När spelaren/fiender flyttar på sig behöver vi beräkna deras nya position och leta igenom alla vår LevelElements för att se om det finns något annat objekt på den platsen man försöker flytta till. Om det finns en vägg eller annat objekt (fiende/spelaren) på platsen måste förflyttningen avbrytas och den tidigare positionen gälla. Notera dock att om spelaren flyttar sig till en plats där det står en fiende så attackerar han denna (mer om detta längre ner). Detsamma gäller om en fiende flyttar sig till platsen där spelaren står. Fiender kan dock inte attackera varandra i spelet.


## Vision range
För att få en effekt av “utforskande” i spelet begränsar vi spelarens synfält till att bara visa objekt inom en radie av 5 tecken (men ni kan också prova med andra radier); Väggarna försvinner dock aldrig när man väl sett dem, men fienderna syns inte så fort de kommer utanför radien.

Avståndet mellan två punkter i 2D kan enkelt beräknas med hjälp av pythagoras sats.


## Kasta tärningar
Spelet använder sig av simulerade tärningskast för att avgöra hur mycket skada spelaren och fienderna gör på varandra.

Skapa en klass “Dice” med en konstruktor som tar 3 parametrar: numberOfDice, sidesPerDice och modifier. Genom att skapa nya instanser av denna kommer man kunna skapa olika uppsättningar av tärningar t.ex “3d6+2”, d.v.s slag med 3 stycken 6-sidiga tärningar, där man tar resultatet och sedan plussar på 2, för att få en total poäng.

Dice-objekt ska ha en publik Throw() metod som returnerar ett heltal med den poäng man får när man slår med tärningarna enligt objektets konfiguration. Varje anrop motsvarar alltså ett nytt kast med tärningarna.

Gör även en override av Dice.ToString(), så att man när man skriver ut ett Dice-objekt får en sträng som beskriver objektets konfiguration. t.ex: “3d6+2”.


## Attack och försvar
När en spelare attackerar (går in i) en fiende, eller tvärtom så behöver vi simulera tärningskast för att få en poäng som avgör hur mycket skada attacken gör. Den som attackerar kastar då sina tärningar först och får en attackpoäng. Sedan kastar den som försvarar sina tärningar och får en försvarspoäng. Ta sedan attackpoängen minus försvarspoängen, och om differensen är större än 0, dra motsvarande antal från förvararens HP (hälsopoäng). Efter en eller flera attacker kommer HP ner till 0, varpå fienden dör (eller spelaren får game over).

Om försvararen överlever kommer han direkt att göra en motattack, d.v.s kasta tärningar på nytt för att få en attackpoäng; varpå den som först attackerade nu försvarar genom att kasta sina tärningar. Dra bort HP enligt reglerna ovan.

Spelaren samt alla typer av fiender har en uppsättning tärningskonfigurationer för sin attack respektive försvar, samt en hårdkodad HP som man startar med. Jag har använt följande konfigurationer, men ni får gärna prova med andra:

- Player: HP = 100, Attack = 2d6+2, Defence = 2d6+0
- Rat: HP = 10, Attack = 1d6+3, Defence = 1d6+1
- Snake: HP = 25, Attack = 3d4+2, Defence = 1d8+5 


## Förflyttningsmönster
Spelaren förflyttar sig 1 steg upp, ner, höger eller vänster varje omgång, alternativt står still, beroende på vilken knapp användaren tryckt på.
 
Rat förflyttar sig 1 steg i slumpmässig vald riktning (upp, ner, höger eller vänster) varje omgång.
 
Snake står still om spelaren är mer än 2 rutor bort, annars förflyttar den sig bort från spelaren.
 
Varken spelare, rats eller snakes kan gå igenom väggar eller varandra.

 
## Redovisning
- Uppgiften ska lösas individuellt.
- Checka in din lösning som ett nytt repo på Github.
- Lämna in uppgiften på ithsdistans med en kommentar med github-länken

### Betygskriterier
Om man vill och har tid får man gärna utöka spelet med fler fiender, eller lägga till “items” som till exempel guld, vapen, rustningar eller mat/dryck (för mer HP). Det går också bra om man vill göra om, eller lägga till fler banor: till exempel kan man ha en utgång ‘>’ som tar en till nästa nivå, och en ‘<’ som tar en till föregående.

Följande kriterier är dock minimum …


**För godkänt:** 

- Appen ska vara kompatibel med, och kunna läsa in filen “Level1.txt”, och korrekt hantera de olika elementen enligt beskrivningen i labben.
- Fiendetyperna rat och snakes ska finnas och ha unika stats. Deras beteenden (rörelsemönster) ska fungera enligt beskrivningen i labben.
- Appen ska ha abstrakta basklasser “LevelElement” och “Enemy”. Klassen “Wall” ärver direkt av “LevelElement”, och klasserna “Rat” och “Snake” ärver direkt av “Enemy” (indirekt av “LevelElement”).
- Spelaren ska kunna flytta ett steg per omgång (upp/ner/höger/vänster), men inte genom väggar eller fiender.
- Spelaren har ett synfält som sträcker sig i en radie 5 steg bort från spelarens position. Fiender som är utanför synfältet syns ej (men uppdateras ändå varje omgång); däremot försvinner inte väggarna när man väl upptäckt dem första gången.
- Går spelaren på en fiende ska attack, defence, och skada avgöras med hjälp av tärningsslag; varpå fienden direkt gör en motattack (om den överlever).
- Går en fiende in i spelaren görs samma sekvens som i föregående punkt, men fienden attackerar först; varpå spelaren gör en motattack (om den överlever).