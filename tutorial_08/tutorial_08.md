## 🔽 Tutorial 08 🔒

**REST Advanced**

---

### Task 1</span>

Using the template **Task1-Template.zip**, apply the following enhancements to the Library service by making use of the respectively best practices presented in the lecture.

1. There are three methods for searching books. These feature several aspects of poor API design. Implement a book search which can find books by any combination of _author, title and genre_.

2. Having fixed the API, one can now concentrate on adding new functionality. In order to still provide some kind of legacy support, we introduce API versions. Implement _Versioning_ for the Library API. Version 1 should provide functionality as of 1.1, the features added in 1.3 – 1.5 should be interfaced in API Version 2 only. All unchanged functionality from version 1 should be available in version 2 as well.

3. Add _Pagination_ for books (both with and without search parameters).

4. In order to help users calculating how many days are left till some date, _e.g. book return date_, implement a method which returns the number of days remaining from **today** till a **specified date**. The return of the method should either be an XML like

   ```xml
   <daysUntil endDate="2015-10-21">42</daysUntil>
   ```

   or an empty HTTP Response with a suitable status code to indicate that the input date could not be successfully parsed.

5. Extend the books API to support both XML and JSON output. Both variants, i.e. using the corresponding HTTP-Header and using either **_.xml_** or **_.json_**, should be available, default to XML if nothing is specified. For the JSON output, obey to the Javascript naming conventions (camel case) while preserving the original pascal case names in your C# source.

6. Make the following calls to your service:
   1. Return books of Genre “fantasy”
   2. Return books by “Tolkien” of Genre “fantasy”
   3. Return books by “Friedman” with Title “flat”
   4. Return all books using API Version 1
   5. Return 6 books starting with the 3rd book available (i.e. books with id 3-8)
   6. Return the first 2 books of Genre “fantasy”
   7. Return number of days until Christmas Eve
   8. Return all books as JSON
   9. Return the first 2 books of Genre “fantasy” as JSON
