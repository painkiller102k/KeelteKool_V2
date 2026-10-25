# Students block

## Purpose

The Students block adds a separate student entity to KeelteKoolV2. It follows the project structure used by the existing Lecturers block while keeping the student interface read-only.

Students can view the student list and student details. For language courses, the `Student` role can use only `Index` and `Details`. Creating, updating, and deleting language courses is limited to the `Lecturer` and `Admin` roles.

## Added components

- `Student` domain model with ID, first name, last name, user ID, creation time and modification time.
- `StudentDTO` for transferring student data to the service layer.
- `IStudentsServices` and `StudentsServices` with create, details, update and delete operations.
- `Students` database table configuration in `KeelteKoolV2Context`.
- EF Core migration `20261025090000_AddStudents`.
- `StudentsController` with read-only `Index` and `Details` actions.
- Read-only Students list and details views.
- Dependency-injection registration for `IStudentsServices`.
- Authentication middleware and role-based authorization on LanguageCourses actions.

## How the Students block works

1. The `StudentsController` reads student records from the database and maps them to `StudentViewModel`.
2. The `StudentsServices` class contains the database operations. It returns `null` for a null DTO or when an ID is not found.
3. The service creates IDs automatically and sets `CreatedAt` and `ModifiedAt` timestamps.
4. Empty first and last names receive safe default values: `Student` and `Unknown`.
5. The authorization attributes in `LanguageCoursesController` allow students to read courses but prevent them from creating, updating or deleting courses.

## Tests

All Students tests are located in `KeelteKoolV2.xUnitTesting/StudentsServicesTests.cs`. Every test has an Estonian XML summary and at least five assertions.

| Test | What is checked | Result |
| --- | --- | --- |
| `Should_CreateStudent_WhenValidDtoIsGiven` | Creates a student and saves valid fields. | Passed |
| `Should_ReturnStudentDetails_WhenStudentExists` | Returns the correct student by ID. | Passed |
| `Should_UpdateStudent_WhenStudentExists` | Updates an existing student's fields. | Passed |
| `Should_DeleteStudent_WhenValidIdIsGiven` | Deletes an existing student. | Passed |
| `Should_ReturnNull_WhenStudentDoesNotExist` | Returns null for an unknown ID and preserves another student. | Passed |
| `Should_ReturnNull_WhenCreatingWithNullDto` | Does not create a student from a null DTO. | Passed |
| `Should_ReturnNull_WhenUpdatingNonExistingStudent` | Does not update a student with an unknown ID. | Passed |
| `Should_ReturnNull_WhenDeletingNonExistingStudent` | Does not delete data for an unknown ID. | Passed |
| `Should_UseDefaultNames_WhenNamesAreEmpty` | Uses default names when both name fields are empty. | Passed |

## Verification

Command used:

```powershell
dotnet test KeelteKoolV2.xUnitTesting/KeelteKoolV2.xUnitTesting.csproj --no-restore
```

Result: **19 tests passed, 0 failed**.

The web project was also built successfully with `dotnet build KeelteKoolV2/KeelteKoolV2.csproj --no-restore`.
