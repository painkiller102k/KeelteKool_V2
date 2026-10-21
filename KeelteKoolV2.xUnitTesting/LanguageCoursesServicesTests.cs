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



    }
}