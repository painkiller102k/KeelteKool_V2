using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
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
            // Arrange
            LecturerDTO dto = new LecturerDTO();
            dto.FirstName = "Test";
            dto.LastName = "Test";
            dto.Qualifications = "Testicles";

            // Act
            var result = await Svc<ILecturersServices>().Create(dto);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("Test", result.FirstName);
            Assert.Equal("Test", result.LastName);
            Assert.Equal("Testicles", result.Qualifications);
        }

        /// <summary>
        /// Kontrollib, et õppejõu andmed leitakse andmebaasist õige ID alusel.
        /// </summary>
        [Fact]
        public async Task Should_GetLecturerDetails_WhenGuidISNotNull()
        {
            // Arrange
            LecturerDTO dto = new LecturerDTO();
            dto.FirstName = "DetailTest";
            dto.LastName = "DetailLast";
            dto.Qualifications = "Master";

            var createdLecturer = await Svc<ILecturersServices>().Create(dto);

            // Act
            var result = await Svc<ILecturersServices>()
                .DetailAsync(createdLecturer.Id);

            // Assert
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
            // Arrange
            LecturerDTO dto = new LecturerDTO();
            dto.FirstName = "UpdateTest";
            dto.LastName = "OldLastName";
            dto.Qualifications = "Bachelor";

            var createdLecturer = await Svc<ILecturersServices>().Create(dto);

            LecturerDTO updatedInfo = new LecturerDTO();
            updatedInfo.Id = createdLecturer.Id;
            updatedInfo.FirstName = "UpdatedFirstName";
            updatedInfo.LastName = "UpdatedLastName";
            updatedInfo.Qualifications = "Master";

            // Act
            var result = await Svc<ILecturersServices>().Update(updatedInfo);

            // Assert
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
            // Arrange
            LecturerDTO dto = new LecturerDTO();
            dto.FirstName = "DeleteTest";
            dto.LastName = "DeleteLast";
            dto.Qualifications = "Bachelor";

            var createdLecturer = await Svc<ILecturersServices>().Create(dto);

            // Act
            var result = await Svc<ILecturersServices>()
                .Delete(createdLecturer.Id);

            var deletedLecturer = await Svc<ILecturersServices>()
                .DetailAsync(createdLecturer.Id);

            // Assert
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
            // Arrange
            LecturerDTO dto = new LecturerDTO();
            dto.FirstName = "ExistingTest";
            dto.LastName = "ExistingLast";
            dto.Qualifications = "Bachelor";

            var existingLecturer = await Svc<ILecturersServices>().Create(dto);
            Guid nonExistingId = Guid.NewGuid();

            // Act
            var result = await Svc<ILecturersServices>()
                .DetailAsync(nonExistingId);

            var existingResult = await Svc<ILecturersServices>()
                .DetailAsync(existingLecturer.Id);

            // Assert
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
            // Arrange
            LecturerDTO dto = new LecturerDTO();
            dto.FirstName = "DeleteExistingTest";
            dto.LastName = "DeleteExistingLast";
            dto.Qualifications = "Master";

            var existingLecturer = await Svc<ILecturersServices>().Create(dto);
            Guid nonExistingId = Guid.NewGuid();

            // Act
            var result = await Svc<ILecturersServices>()
                .Delete(nonExistingId);

            var existingResult = await Svc<ILecturersServices>()
                .DetailAsync(existingLecturer.Id);

            // Assert
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
            // Arrange
            LecturerDTO existingDto = new LecturerDTO();
            existingDto.FirstName = "UpdateExistingTest";
            existingDto.LastName = "UpdateExistingLast";
            existingDto.Qualifications = "Bachelor";

            var existingLecturer = await Svc<ILecturersServices>().Create(existingDto);

            LecturerDTO updateDto = new LecturerDTO();
            updateDto.Id = Guid.NewGuid();
            updateDto.FirstName = "ChangedName";
            updateDto.LastName = "ChangedLast";
            updateDto.Qualifications = "Master";

            // Act
            var result = await Svc<ILecturersServices>().Update(updateDto);

            var existingResult = await Svc<ILecturersServices>()
                .DetailAsync(existingLecturer.Id);

            // Assert
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
            // Arrange
            var service = Svc<ILecturersServices>();

            var lecturer = await service.Create(new LecturerDTO
            {
                FirstName = "Test",
                LastName = "Lecturer",
                Qualifications = "Master"
            });

            // Act
            var result = await service.Create(null!);
            var existingLecturer = await service.DetailAsync(lecturer.Id);

            // Assert
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
            // Arrange
            var service = Svc<ILecturersServices>();

            var lecturer = await service.Create(new LecturerDTO
            {
                FirstName = "Existing",
                LastName = "Lecturer",
                Qualifications = "Master"
            });

            // Act
            var result = await service.DetailAsync(Guid.Empty);
            var existingLecturer = await service.DetailAsync(lecturer.Id);

            // Assert
            Assert.Null(result);
            Assert.NotNull(existingLecturer);
            Assert.Equal(lecturer.Id, existingLecturer.Id);
            Assert.Equal("Existing", existingLecturer.FirstName);
            Assert.Equal("Lecturer", existingLecturer.LastName);
            Assert.Equal("Master", existingLecturer.Qualifications);
        }



    }
}