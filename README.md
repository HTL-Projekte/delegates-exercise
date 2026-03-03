# Delegates Exercise – Lösung

Diese Lösung zeigt die wichtigsten Delegate-Konzepte in C# in einer kompakten, testbaren Form.

## Überblick

- Eigener Delegate-Typ für Operationen mit zwei `int`-Werten
- Übergabe von Funktionen an Methoden (Higher-Order Functions)
- Nutzung von `Predicate`, `Func` und `Action`
- Rückgabe von Funktionen (Closure und Komposition)
- Einfaches Strategy-Prinzip für Preisberechnung

## Was wurde umgesetzt?

### 1) Eigener Delegate

- `IntOperation` ist als Delegate mit Signatur `int (int x, int y)` definiert.
- Damit können z. B. Addieren, Subtrahieren oder Multiplizieren einheitlich übergeben werden.

### 2) Delegate aufrufen

- `Apply(...)` ruft den Delegate direkt auf (`op(x, y)`).
- `ApplyWithInvoke(...)` zeigt die äquivalente Variante mit `op.Invoke(x, y)`.

### 3) Berechnen und loggen

- `CalculateAndLog(...)` führt die übergebene Operation aus,
- protokolliert das Ergebnis als Text (`Result: ...`),
- und gibt den berechneten Wert zurück.

### 4) Standard-Delegates

- `Filter(...)` verwendet `Predicate<T>` zum Auswählen passender Elemente.
- `Map(...)` verwendet `Func<T, TResult>` zum Umwandeln von Werten.
- `ForEach(...)` verwendet `Action<T>` für Seiteneffekte pro Element.

### 5) Funktionen zurückgeben

- `MakeMultiplier(...)` liefert eine Funktion zurück, die mit einem festen Faktor multipliziert (Closure).
- `Compose(...)` verbindet zwei Funktionen zu einer neuen Funktion (`g(f(x))`).

### 6) Strategy mit `Func`

- `CalculatePrice(...)` erhält eine Rabatt-Strategie als Funktion.
- Die Methode selbst bleibt generisch; die Preislogik kommt von außen.

## Qualitätsaspekte der Lösung

- Bei ungültigen Funktionsparametern werden `ArgumentNullException`s geworfen.
- Alle Methoden sind klar getrennt und auf eine Aufgabe fokussiert.
- Die Tests decken Verhalten und Signaturen vollständig ab.

## Tests starten

```bash
dotnet test
```
