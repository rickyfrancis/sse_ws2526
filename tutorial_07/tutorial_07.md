## 🔽 Tutorial 07 🔒

**REST Basic**

---

### Task 1

Answer the following questions briefly and accurately in your own words:

1. What is an architecture style?
2. What are the REST constraints? Which properties do they induce?
3. What is the “Hypermedia” constraint and which examples of its application do you know?

### Task 2

Convert the following fictitious **SOAP-based** service into a **REST/HTTP-based** one:

- If a HTTP standard verb or header exists for the required purpose, use it.
- Reflect relationships between entities in the structure of the URL, not in the message body.

_Fill in the missing information into the following table:_

| **SOAP Operation**                                     | **HTTP Verb** | **URL**                          | **Request Header**       | **Request Body**                                                              | **Response Header**            | **Response Body**                                       |
| :----------------------------------------------------- | :------------ | :------------------------------- | :----------------------- | :---------------------------------------------------------------------------- | :----------------------------- | :------------------------------------------------------ |
| `getAllBooks`                                          | GET           | `/books`                         | Accept: application/json |                                                                               | Content-Type: application/json |                                                         |
| `getAllBooksAsJson`                                    | GET           | `/books.json`                    |                          |                                                                               |                                |                                                         |
| `getBookById(bookId: int; language: {en, de, fr, nl})` | GET           | `/books/{bookId}`                |                          |                                                                               |                                |                                                         |
| `updateBook(bookId: int; book: (title, authors))`      | PUT           | `/books/{bookId}`                |                          | `<book>`<br>`  <title>...</title>`<br>`  <authors>...</authors>`<br>`</book>` |                                |                                                         |
| `getAllCategories`                                     | GET           | `/categories`                    | Accept: application/xml  |                                                                               |                                | `<category>`<br>`  <title>...</title>`<br>`</category>` |
| `getBooksInCategory(categoryId: int)`                  | GET           | `/categories/{categoryId}/books` |                          |                                                                               | Content-Type: application/xml  |
| `addBook(categoryId: int; book: (title, authors))`     | POST          | `/categories/{categoryId}/books` |                          | `<book>`<br>`  <title>...</title>`<br>`  <authors>...</authors>`<br>`</book>` |                                |                                                         |

### Task 3

In the template **Task3-Template.zip** you will find a REST/HTTP Web service operating on a single resource – **\*User** = (int id, string name)\*. Extend the service towards management of user bookmarks, with **\*Bookmark** = (int id, string url)\*. Following operations/functionality should be implemented:

- Read all bookmarks / Search bookmarks by keyword
- Create a bookmark assigned to a user
- Delete a bookmark
- Read bookmarks of a given user

Extend the service and the client with the following functionality:

- Bookmarks should be able to be assigned to **\*Categories** = (int id, string name)\*.
- All categories can be listed
- Bookmarks of a given category can be listed
- Categories can be searched by a keyword
