Sandcastle Presentation project has a few extensions available which enhance generated documentation.

### Biblio extension ###

This version of Sandcastle contains a small extension to MAML that was created to express bibliographical references in conceptual documents.

```
<bib:book xml:lang="en-us" xmlns:bib="http://sandcastle.codeplex.com/schemas/2010/biblio">
          <bib:title>Rework</bib:title>
          <bib:author>J. Fried, D. Hienemeier Hansson</bib:author>
          <bib:publisher>Crown Business</bib:publisher>
          <bib:year>2010</bib:year>
          <bib:isbn>978-0-307-46374-6</bib:isbn>
          <bib:url>http://www.amazon.com/Rework-Jason-Fried/dp/0307463745</bib:url>
</bib:book>
```

### Miranda history extension ###

Conceptual documentation can contain an XML history exported from Miranda using the [History++](http://themiron.miranda.im/) plugin.