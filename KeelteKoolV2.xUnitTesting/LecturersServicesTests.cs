using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using Xunit;

namespace KeelteKoolV2.xUnitTesting
{
    public class LecturersServicesTests : TestBase
    {
        /// <summary>
        /// Kontrollib, et uue õppejõu loomisel salvestatakse tema andmed õigesti.
        /// </summary>
        [Fact]
        public async Task Should_AddNewLecturer_WhenResultISReturned()
        {
            // ülesseade
            LecturerDTO dto = MockLecturerDTOData();

            // tegevus
            var result = await Svc<ILecturersServices>().Create(dto);

            // kontroll
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("Test", result.FirstName);
            Assert.Equal("Test", result.LastName);
            Assert.Equal("Bachelor", result.Qualifications);
        }

        /// <summary>
        /// Kontrollib, et õppejõu andmed leitakse andmebaasist õige ID alusel.
        /// </summary>
        [Fact]
        public async Task Should_GetLecturerDetails_WhenGuidISNotNull()
        {
            // ülesseade
            LecturerDTO dto = new LecturerDTO
            {
                FirstName = "DetailTest",
                LastName = "DetailLast",
                Qualifications = "Master"
            };

            var createdLecturer = await Svc<ILecturersServices>().Create(dto);

            // tegevus
            var result = await Svc<ILecturersServices>()
                .DetailAsync(createdLecturer!.Id);

            // kontroll
            Assert.NotNull(result);
            Assert.Equal(createdLecturer.Id, result.Id);
            Assert.Equal("DetailTest", result.FirstName);
            Assert.Equal("DetailLast", result.LastName);
            Assert.Equal("Master", result.Qualifications);
            Assert.NotEqual(Guid.Empty, result.Id);
        }

        /// <summary>
        /// Kontrollib, et õppejõu andmeid saab andmebaasis edukalt uuendada.
        /// </summary>
        [Fact]
        public async Task Should_UpdateLecturerWithNewData_WhenDataIsDifferentFromDB()
        {
            // ülesseade
            LecturerDTO dto = new LecturerDTO
            {
                FirstName = "UpdateTest",
                LastName = "OldLastName",
                Qualifications = "Bachelor"
            };

            var createdLecturer = await Svc<ILecturersServices>().Create(dto);

            LecturerDTO updatedInfo = new LecturerDTO
            {
                Id = createdLecturer!.Id,
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                Qualifications = "Master"
            };

            // tegevus
            var result = await Svc<ILecturersServices>().Update(updatedInfo);

            // kontroll
            Assert.NotNull(result);
            Assert.Equal(createdLecturer.Id, result.Id);
            Assert.Equal("UpdatedFirstName", result.FirstName);
            Assert.Equal("UpdatedLastName", result.LastName);
            Assert.Equal("Master", result.Qualifications);
            Assert.NotEqual("OldLastName", result.LastName);
        }

        /// <summary>
        /// Kontrollib, et õppejõud kustutatakse andmebaasist õige ID alusel.
        /// </summary>
        [Fact]
        public async Task Should_DeleteLecturer_WhenValidIDIsGiven()
        {
            // ülesseade
            LecturerDTO dto = new LecturerDTO
            {
                FirstName = "DeleteTest",
                LastName = "DeleteLast",
                Qualifications = "Bachelor"
            };

            var createdLecturer = await Svc<ILecturersServices>().Create(dto);

            // tegevus
            var result = await Svc<ILecturersServices>()
                .Delete(createdLecturer!.Id);

            var deletedLecturer = await Svc<ILecturersServices>()
                .DetailAsync(createdLecturer.Id);

            // kontroll
            Assert.NotNull(result);
            Assert.Equal(createdLecturer.Id, result.Id);
            Assert.Equal("DeleteTest", result.FirstName);
            Assert.Equal("DeleteLast", result.LastName);
            Assert.Equal("Bachelor", result.Qualifications);
            Assert.Null(deletedLecturer);
        }

        /// <summary>
        /// Kontrollib, et olematu ID korral ei leita andmebaasist õppejõudu.
        /// </summary>
        [Fact]
        public async Task Should_ReturnNull_WhenLecturerDoesNotExist()
        {
            // ülesseade
            LecturerDTO dto = new LecturerDTO
            {
                FirstName = "ExistingTest",
                LastName = "ExistingLast",
                Qualifications = "Bachelor"
            };

            var existingLecturer = await Svc<ILecturersServices>().Create(dto);
            Guid nonExistingId = Guid.NewGuid();

            // tegevus
            var result = await Svc<ILecturersServices>()
                .DetailAsync(nonExistingId);

            var existingResult = await Svc<ILecturersServices>()
                .DetailAsync(existingLecturer!.Id);

            // kontroll
            Assert.Null(result);
            Assert.NotNull(existingResult);
            Assert.Equal(existingLecturer.Id, existingResult.Id);
            Assert.Equal("ExistingTest", existingResult.FirstName);
            Assert.Equal("ExistingLast", existingResult.LastName);
        }

        /// <summary>
        /// Kontrollib, et olematu ID korral ei kustutata ühtegi õppejõudu.
        /// </summary>
        [Fact]
        public async Task Should_ReturnNull_WhenDeletingNonExistingLecturer()
        {
            // ülesseade
            LecturerDTO dto = new LecturerDTO
            {
                FirstName = "DeleteExistingTest",
                LastName = "DeleteExistingLast",
                Qualifications = "Master"
            };

            var existingLecturer = await Svc<ILecturersServices>().Create(dto);
            Guid nonExistingId = Guid.NewGuid();

            // tegevus
            var result = await Svc<ILecturersServices>()
                .Delete(nonExistingId);

            var existingResult = await Svc<ILecturersServices>()
                .DetailAsync(existingLecturer!.Id);

            // kontroll
            Assert.Null(result);
            Assert.NotNull(existingResult);
            Assert.Equal(existingLecturer.Id, existingResult.Id);
            Assert.Equal("DeleteExistingTest", existingResult.FirstName);
            Assert.Equal("DeleteExistingLast", existingResult.LastName);
        }

        /// <summary>
        /// Kontrollib, et olematu ID-ga õppejõu uuendamisel tagastatakse null.
        /// </summary>
        [Fact]
        public async Task Should_ReturnNull_WhenUpdatingNonExistingLecturer()
        {
            // ülesseade
            LecturerDTO existingDto = new LecturerDTO
            {
                FirstName = "UpdateExistingTest",
                LastName = "UpdateExistingLast",
                Qualifications = "Bachelor"
            };

            var existingLecturer = await Svc<ILecturersServices>().Create(existingDto);

            LecturerDTO updateDto = new LecturerDTO
            {
                Id = Guid.NewGuid(),
                FirstName = "ChangedName",
                LastName = "ChangedLast",
                Qualifications = "Master"
            };

            // tegevus
            var result = await Svc<ILecturersServices>().Update(updateDto);

            var existingResult = await Svc<ILecturersServices>()
                .DetailAsync(existingLecturer!.Id);

            // kontroll
            Assert.Null(result);
            Assert.NotNull(existingResult);
            Assert.Equal(existingLecturer.Id, existingResult.Id);
            Assert.Equal("UpdateExistingTest", existingResult.FirstName);
            Assert.Equal("UpdateExistingLast", existingResult.LastName);
            Assert.Equal("Bachelor", existingResult.Qualifications);
        }

        /// <summary>
        /// Kontrollib, et null väärtusega DTO korral ei looda uut õppejõudu
        /// ega muudeta olemasolevaid andmeid.
        /// </summary>
        [Fact]
        public async Task Should_ReturnNull_WhenCreatingWithNullDto()
        {
            // ülesseade
            var service = Svc<ILecturersServices>();

            var lecturer = await service.Create(new LecturerDTO
            {
                FirstName = "Test",
                LastName = "Lecturer",
                Qualifications = "Master"
            });

            // tegevus
            var result = await service.Create(null!);
            var existingLecturer = await service.DetailAsync(lecturer!.Id);

            // kontroll
            Assert.Null(result);
            Assert.NotNull(existingLecturer);
            Assert.Equal(lecturer.Id, existingLecturer.Id);
            Assert.Equal("Test", existingLecturer.FirstName);
            Assert.Equal("Lecturer", existingLecturer.LastName);
            Assert.Equal("Master", existingLecturer.Qualifications);
        }

        /// <summary>
        /// Kontrollib, et tühja GUID väärtuse korral õppejõudu ei leita.
        /// </summary>
        [Fact]
        public async Task Should_ReturnNull_WhenSearchingWithEmptyGuid()
        {
            // ülesseade
            var service = Svc<ILecturersServices>();

            var lecturer = await service.Create(new LecturerDTO
            {
                FirstName = "Existing",
                LastName = "Lecturer",
                Qualifications = "Master"
            });

            // tegevus
            var result = await service.DetailAsync(Guid.Empty);
            var existingLecturer = await service.DetailAsync(lecturer!.Id);

            // kontroll
            Assert.Null(result);
            Assert.NotNull(existingLecturer);
            Assert.Equal(lecturer.Id, existingLecturer.Id);
            Assert.Equal("Existing", existingLecturer.FirstName);
            Assert.Equal("Lecturer", existingLecturer.LastName);
            Assert.Equal("Master", existingLecturer.Qualifications);
        }

        /// <summary>
        /// Kontrollib, et tühjad ees- ja perekonnanimed asendatakse vaikeväärtustega.
        /// </summary>
        [Fact]
        public async Task Should_UseDefaultNames_WhenFirstNameAndLastNameAreEmpty()
        {
            // ülesseade
            var service = Svc<ILecturersServices>();

            LecturerDTO dto = new LecturerDTO
            {
                FirstName = "",
                LastName = "",
                Qualifications = "Master"
            };

            // tegevus
            var result = await service.Create(dto);

            // kontroll
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("Martin", result.FirstName);
            Assert.Equal("Rossakov", result.LastName);
            Assert.Equal("Master", result.Qualifications);
        }

        /// <summary>
        /// Kontrollib, et õppejõu loomisel salvestatakse kasutaja ID ja kuupäevad.
        /// </summary>
        [Fact]
        public async Task Should_SaveUserIdAndDates_WhenCreatingLecturer()
        {
            // ülesseade
            var userId = "test-user-123";
            var beforeCreate = DateTime.Now;

            LecturerDTO dto = new LecturerDTO
            {
                FirstName = "DateTest",
                LastName = "Lecturer",
                Qualifications = "Master",
                UserID = userId
            };

            // tegevus
            var result = await Svc<ILecturersServices>().Create(dto);
            var afterCreate = DateTime.Now;

            // kontroll
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(userId, result.UserID);
            Assert.True(result.CreatedAt >= beforeCreate);
            Assert.True(result.CreatedAt <= afterCreate);
            Assert.True(result.ModifiedAt >= beforeCreate);
            Assert.True(result.ModifiedAt <= afterCreate);
        }

        /// <summary>
        /// Kontrollib, et null väärtusega DTO korral ei uuendata õppejõu andmeid.
        /// </summary>
        [Fact]
        public async Task Should_ReturnNull_WhenUpdatingWithNullDto()
        {
            // ülesseade
            var service = Svc<ILecturersServices>();

            var lecturer = await service.Create(new LecturerDTO
            {
                FirstName = "Original",
                LastName = "Lecturer",
                Qualifications = "Bachelor",
                UserID = "original-user"
            });

            // tegevus
            var result = await service.Update(null!);
            var existing = await service.DetailAsync(lecturer!.Id);

            // kontroll
            Assert.Null(result);
            Assert.NotNull(existing);
            Assert.Equal(lecturer.Id, existing.Id);
            Assert.Equal("Original", existing.FirstName);
            Assert.Equal("Lecturer", existing.LastName);
            Assert.Equal("Bachelor", existing.Qualifications);
            Assert.Equal("original-user", existing.UserID);
        }

        /// <summary>
        /// Kontrollib, et null väärtusega ees- ja perekonnanimed
        /// asendatakse õppejõu loomisel vaikeväärtustega.
        /// </summary>
        [Fact]
        public async Task Should_UseDefaultNames_WhenFirstNameAndLastNameAreNull()
        {
            // ülesseade
            var service = Svc<ILecturersServices>();

            LecturerDTO dto = new LecturerDTO
            {
                FirstName = null!,
                LastName = null!,
                Qualifications = "Master",
                UserID = "null-name-test"
            };

            // tegevus
            var result = await service.Create(dto);

            // kontroll
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("Martin", result.FirstName);
            Assert.Equal("Rossakov", result.LastName);
            Assert.Equal("Master", result.Qualifications);
            Assert.Equal("null-name-test", result.UserID);

            var saved = await service.DetailAsync(result.Id);

            Assert.NotNull(saved);
            Assert.Equal(result.Id, saved.Id);
            Assert.Equal("Martin", saved.FirstName);
        }

        /// <summary>
        /// Kontrollib, et loodud õppejõu andmed säilivad andmebaasis.
        /// </summary>
        [Fact]
        public async Task Should_SaveLecturerAndRetrieveItFromDatabase()
        {
            // ülesseade
            var service = Svc<ILecturersServices>();
            LecturerDTO dto = MockLecturerDTOData();
            dto.UserID = "saved-test-user";

            // tegevus
            var created = await service.Create(dto);
            var saved = await service.DetailAsync(created!.Id);

            // kontroll
            Assert.NotNull(created);
            Assert.NotEqual(Guid.Empty, created.Id);
            Assert.NotNull(saved);
            Assert.Equal(created.Id, saved.Id);
            Assert.Equal("Test", saved.FirstName);
            Assert.Equal("Test", saved.LastName);
            Assert.Equal("Bachelor", saved.Qualifications);
            Assert.Equal("saved-test-user", saved.UserID);
        }

        /// <summary>
        /// Kontrollib, et õppejõu uuendatud andmed säilivad andmebaasis.
        /// </summary>
        [Fact]
        public async Task Should_SaveUpdatedLecturerAndRetrieveItFromDatabase()
        {
            // ülesseade
            var service = Svc<ILecturersServices>();
            LecturerDTO dto = new LecturerDTO
            {
                FirstName = "BeforeUpdate",
                LastName = "OldLastName",
                Qualifications = "Bachelor"
            };

            var created = await service.Create(dto);

            LecturerDTO updateDto = new LecturerDTO
            {
                Id = created!.Id,
                FirstName = "AfterUpdate",
                LastName = "NewLastName",
                Qualifications = "Master"
            };

            // tegevus
            var updated = await service.Update(updateDto);
            var saved = await service.DetailAsync(created.Id);

            // kontroll
            Assert.NotNull(updated);
            Assert.Equal(created.Id, updated.Id);
            Assert.Equal("AfterUpdate", updated.FirstName);
            Assert.Equal("NewLastName", updated.LastName);
            Assert.Equal("Master", updated.Qualifications);
            Assert.NotNull(saved);
            Assert.Equal("AfterUpdate", saved.FirstName);
            Assert.Equal("NewLastName", saved.LastName);
            Assert.Equal("Master", saved.Qualifications);
        }

        /// <summary>
        /// Loob testimiseks õppejõu DTO näidisandmed.
        /// </summary>
        private static LecturerDTO MockLecturerDTOData()
        {
            return new LecturerDTO
            {
                FirstName = "Test",
                LastName = "Test",
                Qualifications = "Bachelor"
            };
        }
    }
}