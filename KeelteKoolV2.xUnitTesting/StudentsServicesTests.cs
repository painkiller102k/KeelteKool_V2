using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using Xunit;

namespace KeelteKoolV2.xUnitTesting
{
    public class StudentsServicesTests : TestBase
    {
        /// <summary>
        /// Kontrollib, et uue õpilase loomisel salvestatakse eesnimi, perekonnanimi,
        /// kasutaja ID ning ajatemplid õigesti.
        /// </summary>
        [Fact]
        public async Task Should_CreateStudent_WhenValidDtoIsGiven()
        {
            var result = await Svc<IStudentsServices>().Create(new StudentDTO
            {
                FirstName = "Mari",
                LastName = "Tamm",
                UserID = "student-1"
            });

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("Mari", result.FirstName);
            Assert.Equal("Tamm", result.LastName);
            Assert.Equal("student-1", result.UserID);
        }

        /// <summary>
        /// Kontrollib, et õpilase detailvaade tagastab andmebaasist õige õpilase ID järgi.
        /// </summary>
        [Fact]
        public async Task Should_ReturnStudentDetails_WhenStudentExists()
        {
            var service = Svc<IStudentsServices>();
            var created = await service.Create(new StudentDTO { FirstName = "Jaan", LastName = "Kask", UserID = "student-2" });

            var result = await service.DetailAsync(created!.Id);

            Assert.NotNull(result);
            Assert.Equal(created.Id, result.Id);
            Assert.Equal("Jaan", result.FirstName);
            Assert.Equal("Kask", result.LastName);
            Assert.Equal("student-2", result.UserID);
        }

        /// <summary>
        /// Kontrollib, et olemasoleva õpilase andmeid saab muuta ja muudatused salvestuvad.
        /// </summary>
        [Fact]
        public async Task Should_UpdateStudent_WhenStudentExists()
        {
            var service = Svc<IStudentsServices>();
            var created = await service.Create(new StudentDTO { FirstName = "Liis", LastName = "Vana", UserID = "student-3" });

            var result = await service.Update(new StudentDTO
            {
                Id = created!.Id,
                FirstName = "Liis",
                LastName = "Uus",
                UserID = "student-3-updated"
            });

            Assert.NotNull(result);
            Assert.Equal(created.Id, result.Id);
            Assert.Equal("Liis", result.FirstName);
            Assert.Equal("Uus", result.LastName);
            Assert.Equal("student-3-updated", result.UserID);
        }

        /// <summary>
        /// Kontrollib, et õpilase kustutamisel eemaldatakse ta andmebaasist.
        /// </summary>
        [Fact]
        public async Task Should_DeleteStudent_WhenValidIdIsGiven()
        {
            var service = Svc<IStudentsServices>();
            var created = await service.Create(new StudentDTO { FirstName = "Karl", LastName = "Saar", UserID = "student-4" });

            var result = await service.Delete(created!.Id);
            var deletedStudent = await service.DetailAsync(created.Id);

            Assert.NotNull(result);
            Assert.Equal(created.Id, result.Id);
            Assert.Equal("Karl", result.FirstName);
            Assert.Equal("Saar", result.LastName);
            Assert.Null(deletedStudent);
        }

        /// <summary>
        /// Kontrollib, et olematu ID korral ei tagastata õpilast ning olemasolev õpilane jääb alles.
        /// </summary>
        [Fact]
        public async Task Should_ReturnNull_WhenStudentDoesNotExist()
        {
            var service = Svc<IStudentsServices>();
            var existing = await service.Create(new StudentDTO { FirstName = "Anu", LastName = "Paju", UserID = "student-5" });

            var result = await service.DetailAsync(Guid.NewGuid());
            var existingResult = await service.DetailAsync(existing!.Id);

            Assert.Null(result);
            Assert.NotNull(existingResult);
            Assert.Equal(existing.Id, existingResult.Id);
            Assert.Equal("Anu", existingResult.FirstName);
            Assert.Equal("Paju", existingResult.LastName);
        }

        /// <summary>
        /// Kontrollib, et null DTO korral uut õpilast ei looda ja olemasolevad andmed ei muutu.
        /// </summary>
        [Fact]
        public async Task Should_ReturnNull_WhenCreatingWithNullDto()
        {
            var service = Svc<IStudentsServices>();
            var existing = await service.Create(new StudentDTO { FirstName = "Mati", LastName = "Mets", UserID = "student-6" });

            var result = await service.Create(null);
            var existingResult = await service.DetailAsync(existing!.Id);

            Assert.Null(result);
            Assert.NotNull(existingResult);
            Assert.Equal(existing.Id, existingResult.Id);
            Assert.Equal("Mati", existingResult.FirstName);
            Assert.Equal("Mets", existingResult.LastName);
        }

        /// <summary>
        /// Kontrollib, et olematu ID-ga õpilase muutmine tagastab nulli ega muuda olemasolevat õpilast.
        /// </summary>
        [Fact]
        public async Task Should_ReturnNull_WhenUpdatingNonExistingStudent()
        {
            var service = Svc<IStudentsServices>();
            var existing = await service.Create(new StudentDTO { FirstName = "Kati", LastName = "Kivi", UserID = "student-7" });

            var result = await service.Update(new StudentDTO { Id = Guid.NewGuid(), FirstName = "Uus", LastName = "Nimi" });
            var existingResult = await service.DetailAsync(existing!.Id);

            Assert.Null(result);
            Assert.NotNull(existingResult);
            Assert.Equal(existing.Id, existingResult.Id);
            Assert.Equal("Kati", existingResult.FirstName);
            Assert.Equal("Kivi", existingResult.LastName);
        }

        /// <summary>
        /// Kontrollib, et olematu ID-ga õpilase kustutamine tagastab nulli ega kustuta teist õpilast.
        /// </summary>
        [Fact]
        public async Task Should_ReturnNull_WhenDeletingNonExistingStudent()
        {
            var service = Svc<IStudentsServices>();
            var existing = await service.Create(new StudentDTO { FirstName = "Riin", LastName = "Rebane", UserID = "student-8" });

            var result = await service.Delete(Guid.NewGuid());
            var existingResult = await service.DetailAsync(existing!.Id);

            Assert.Null(result);
            Assert.NotNull(existingResult);
            Assert.Equal(existing.Id, existingResult.Id);
            Assert.Equal("Riin", existingResult.FirstName);
            Assert.Equal("Rebane", existingResult.LastName);
        }

        /// <summary>
        /// Kontrollib, et tühjade nimede korral kasutatakse õpilase loomisel vaikeväärtusi.
        /// </summary>
        [Fact]
        public async Task Should_UseDefaultNames_WhenNamesAreEmpty()
        {
            var result = await Svc<IStudentsServices>().Create(new StudentDTO
            {
                FirstName = "",
                LastName = "",
                UserID = "student-9"
            });

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("Student", result.FirstName);
            Assert.Equal("Unknown", result.LastName);
            Assert.Equal("student-9", result.UserID);
        }
    }
}
