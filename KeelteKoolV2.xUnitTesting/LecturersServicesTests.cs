using KeelteKoolV2.Core.Domain;
using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Data;
using Xunit;

namespace KeelteKoolV2.xUnitTesting
{
    public class LecturersServicesTests : TestBase
    {
        // PÕHITESTID (LanguageCourse eeskujul)

        /// <summary>
        /// Kontrollib, et uue õppejõu lisamisel tagastatakse objekt,
        /// millel on uus ID ning samad andmed, mis sisestati.
        /// </summary>
        [Fact]
        public async Task Should_AddNewLecturer_WhenResultIsReturned()
        {
            //ülesseade
            LecturerDTO newLecturerDTO = new LecturerDTO();
            newLecturerDTO.FirstName = "Mari";
            newLecturerDTO.LastName = "Maasikas";
            newLecturerDTO.Qualifications = "Magistrikraad";
            newLecturerDTO.UserID = "test-user";

            //tegevus
            var result = await Svc<ILecturersServices>().Create(newLecturerDTO);

            //kontroll
            Assert.NotNull(result); //teenus tagastas objekti
            Assert.NotEqual(Guid.Empty, result.Id); //teenus genereeris uue id
            Assert.Equal(newLecturerDTO.FirstName, result.FirstName); //andmed ei kadunud vahepeal ära
            Assert.Equal(newLecturerDTO.LastName, result.LastName);
            Assert.Equal(newLecturerDTO.Qualifications, result.Qualifications);
            Assert.Equal(newLecturerDTO.UserID, result.UserID);
        }

        /// <summary>
        /// Negatiivne test: kontrollib, et tühja ees- ja perekonnanimega
        /// õppejõudu andmebaasi ei lisata.
        /// </summary>
        [Fact]
        public async Task ShouldNot_AddNewLecturer_WhenFieldsEmpty()
        {
            //ülesseade
            LecturerDTO newLecturer = MockLecturerDTOData();
            newLecturer.FirstName = string.Empty;
            newLecturer.LastName = string.Empty;

            //tegevus
            var result = await Svc<ILecturersServices>().Create(newLecturer);

            //kontroll
            Assert.Null(result); //teenus lükkas objekti lisamise tagasi
            Assert.Empty(Svc<KeelteKoolV2Context>().Lecturers); //andmebaasi ei lisatud midagi
            //teenus ei muutnud sisendandmeid ega pannud neile vaikeväärtusi
            Assert.Equal(string.Empty, newLecturer.FirstName);
            Assert.Equal(string.Empty, newLecturer.LastName);
            Assert.Equal("Bakalaureusekraad", newLecturer.Qualifications);
        }

        /// <summary>
        /// Kontrollib, et andmebaasi lisatud õppejõud leitakse
        /// DetailAsync meetodiga tema ID järgi üles.
        /// </summary>
        [Fact]
        public async Task Should_ReturnLecturerDetails_WhenGuidIsNotNull()
        {
            //ülesseade
            Lecturer createdLecturer = await AddObjectToDB();

            //tegevus
            //kasutame objektis asuvat id et see objekt tagasi lugeda andmebaasist DetailAsync meetodiga
            var result = await Svc<ILecturersServices>().DetailAsync(createdLecturer.Id);

            //kontroll
            Assert.NotNull(result); //tagastati midagi
            Assert.Equal(createdLecturer.Id, result.Id); //tagastati sama id-ga objekt
            Assert.Equal(createdLecturer, result); //objekt on sama, mis andmebaasi lisati
            Assert.Equal("Test", result.FirstName);
            Assert.Equal("Õppejõud", result.LastName);
            Assert.Equal("Bakalaureusekraad", result.Qualifications);
        }

        /// <summary>
        /// Kontrollib, et õppejõu andmete muutmisel salvestatakse uued andmed,
        /// loomise aeg jääb samaks ja muutmise aeg uueneb.
        /// </summary>
        [Fact]
        public async Task Should_UpdateLecturerWithNewData_WhenDataIsDifferentFromDB()
        {
            //ülesseade
            Lecturer createdLecturer = await AddObjectToDB();
            //jätame vanad väärtused meelde, sest teenus muudab sama objekti
            DateTime oldCreatedAt = createdLecturer.CreatedAt;
            DateTime oldModifiedAt = createdLecturer.ModifiedAt;

            var dto = new LecturerDTO();
            dto.Id = createdLecturer.Id;
            dto.FirstName = "Jaan";
            dto.LastName = "Tamm";
            dto.Qualifications = "Doktorikraad";
            dto.UserID = createdLecturer.UserID;

            //tegevus
            var result = await Svc<ILecturersServices>().Update(dto);

            //kontroll
            Assert.NotNull(result);
            Assert.Equal(dto.Id, result.Id); //uuendati sama objekti
            Assert.Equal(dto.FirstName, result.FirstName); //uued andmed on salvestatud
            Assert.Equal(dto.LastName, result.LastName);
            Assert.Matches(dto.Qualifications, result.Qualifications);
            Assert.Equal(oldCreatedAt, result.CreatedAt); //loomise aeg ei muutunud
            Assert.True(result.ModifiedAt >= oldModifiedAt); //muutmise aeg uuenes
        }

        /// <summary>
        /// Kontrollib, et õige ID korral õppejõud kustutatakse
        /// ja teda ei leita enam andmebaasist.
        /// </summary>
        [Fact]
        public async Task Should_DeleteDataFromDB_WhenValidIDIsGiven()
        {
            //ülesseade
            Lecturer createdLecturer = await AddObjectToDB();

            //tegevus
            var deletedLecturer = await Svc<ILecturersServices>().Delete(createdLecturer.Id);
            var result = await Svc<ILecturersServices>().DetailAsync(createdLecturer.Id);

            //kontroll
            Assert.Null(result); //objekti ei leita enam üles
            Assert.NotNull(deletedLecturer); //teenus tagastas kustutatud objekti
            Assert.Equal(createdLecturer, deletedLecturer); //kustutati just see objekt
            Assert.Equal(createdLecturer.Id, deletedLecturer.Id);
            Assert.NotEqual(deletedLecturer, result);
            Assert.Empty(Svc<KeelteKoolV2Context>().Lecturers); //andmebaas on tühi
        }

        // OMAD TESTID

        /// <summary>
        /// Negatiivne test: kontrollib, et olematu ID korral õppejõudu ei leita,
        /// kuid olemasolev õppejõud leitakse endiselt.
        /// </summary>
        [Fact]
        public async Task ShouldNot_ReturnLecturer_WhenIdDoesNotExist()
        {
            //ülesseade
            Lecturer existingLecturer = await AddObjectToDB();
            Guid nonExistingId = Guid.NewGuid();

            //tegevus
            var result = await Svc<ILecturersServices>().DetailAsync(nonExistingId);
            var existingResult = await Svc<ILecturersServices>().DetailAsync(existingLecturer.Id);

            //kontroll
            Assert.Null(result); //olematut objekti ei leitud
            Assert.NotEqual(existingLecturer.Id, nonExistingId);
            Assert.NotNull(existingResult); //olemasolev objekt on alles
            Assert.Equal(existingLecturer.Id, existingResult.Id);
            Assert.Equal("Test", existingResult.FirstName);
        }

        /// <summary>
        /// Negatiivne test: kontrollib, et olematu ID korral ei kustutata
        /// ühtegi õppejõudu ja olemasolevad andmed jäävad alles.
        /// </summary>
        [Fact]
        public async Task ShouldNot_DeleteAnyLecturer_WhenIdDoesNotExist()
        {
            //ülesseade
            Lecturer existingLecturer = await AddObjectToDB();

            //tegevus
            var result = await Svc<ILecturersServices>().Delete(Guid.NewGuid());
            var existingResult = await Svc<ILecturersServices>().DetailAsync(existingLecturer.Id);

            //kontroll
            Assert.Null(result); //midagi ei kustutatud
            Assert.Single(Svc<KeelteKoolV2Context>().Lecturers); //andmebaasis on endiselt 1 õppejõud
            Assert.NotNull(existingResult);
            Assert.Equal(existingLecturer.Id, existingResult.Id);
            Assert.Equal("Õppejõud", existingResult.LastName);
        }

        /// <summary>
        /// Negatiivne test: kontrollib, et olematu ID-ga õppejõu uuendamisel
        /// tagastatakse null ja olemasoleva õppejõu andmed ei muutu.
        /// </summary>
        [Fact]
        public async Task ShouldNot_UpdateLecturer_WhenIdDoesNotExist()
        {
            //ülesseade
            Lecturer existingLecturer = await AddObjectToDB();
            LecturerDTO dto = new LecturerDTO
            {
                Id = Guid.NewGuid(),
                FirstName = "Muudetud",
                LastName = "Nimi",
                Qualifications = "Doktorikraad"
            };

            //tegevus
            var result = await Svc<ILecturersServices>().Update(dto);
            var existingResult = await Svc<ILecturersServices>().DetailAsync(existingLecturer.Id);

            //kontroll
            Assert.Null(result); //midagi ei uuendatud
            Assert.NotNull(existingResult);
            Assert.Equal("Test", existingResult.FirstName); //vanad andmed on alles
            Assert.Equal("Õppejõud", existingResult.LastName);
            Assert.Equal("Bakalaureusekraad", existingResult.Qualifications);
            Assert.Single(Svc<KeelteKoolV2Context>().Lecturers); //uut objekti ei tekkinud
        }

        /// <summary>
        /// Negatiivne test: kontrollib, et null väärtusega DTO korral
        /// uut õppejõudu ei looda ega muudeta olemasolevaid andmeid.
        /// </summary>
        [Fact]
        public async Task ShouldNot_AddNewLecturer_WhenDtoIsNull()
        {
            //ülesseade
            Lecturer existingLecturer = await AddObjectToDB();

            //tegevus
            var result = await Svc<ILecturersServices>().Create(null!);
            var existingResult = await Svc<ILecturersServices>().DetailAsync(existingLecturer.Id);

            //kontroll
            Assert.Null(result); //uut objekti ei loodud
            Assert.Single(Svc<KeelteKoolV2Context>().Lecturers); //andmebaasis on ainult varasem objekt
            Assert.NotNull(existingResult);
            Assert.Equal(existingLecturer.Id, existingResult.Id);
            Assert.Equal("Test", existingResult.FirstName);
        }

        /// <summary>
        /// Negatiivne test: kontrollib, et null väärtusega DTO korral
        /// õppejõu andmeid ei uuendata.
        /// </summary>
        [Fact]
        public async Task ShouldNot_UpdateLecturer_WhenDtoIsNull()
        {
            //ülesseade
            Lecturer existingLecturer = await AddObjectToDB();
            DateTime oldModifiedAt = existingLecturer.ModifiedAt;

            //tegevus
            var result = await Svc<ILecturersServices>().Update(null!);
            var existingResult = await Svc<ILecturersServices>().DetailAsync(existingLecturer.Id);

            //kontroll
            Assert.Null(result); //midagi ei uuendatud
            Assert.NotNull(existingResult);
            Assert.Equal("Test", existingResult.FirstName); //andmed on samad
            Assert.Equal("Õppejõud", existingResult.LastName);
            Assert.Equal("Bakalaureusekraad", existingResult.Qualifications);
            Assert.Equal(oldModifiedAt, existingResult.ModifiedAt); //muutmise aeg ei muutunud
        }

        /// <summary>
        /// Negatiivne test: kontrollib, et tühja GUID-i (Guid.Empty) korral
        /// õppejõudu ei leita.
        /// </summary>
        [Fact]
        public async Task ShouldNot_ReturnLecturer_WhenGuidIsEmpty()
        {
            //ülesseade
            Lecturer existingLecturer = await AddObjectToDB();

            //tegevus
            var result = await Svc<ILecturersServices>().DetailAsync(Guid.Empty);
            var existingResult = await Svc<ILecturersServices>().DetailAsync(existingLecturer.Id);

            //kontroll
            Assert.Null(result); //tühja id-ga objekti ei leitud
            Assert.NotEqual(Guid.Empty, existingLecturer.Id); //lisatud objektil ei ole tühi id
            Assert.NotNull(existingResult);
            Assert.Equal(existingLecturer.Id, existingResult.Id);
            Assert.Equal("Test", existingResult.FirstName);
        }

        /// <summary>
        /// Negatiivne test: kontrollib, et null väärtusega ees- ja perekonnanime
        /// korral õppejõudu andmebaasi ei lisata.
        /// </summary>
        [Fact]
        public async Task ShouldNot_AddNewLecturer_WhenFieldsAreNull()
        {
            //ülesseade
            LecturerDTO newLecturer = MockLecturerDTOData();
            newLecturer.FirstName = null!;
            newLecturer.LastName = null!;

            //tegevus
            var result = await Svc<ILecturersServices>().Create(newLecturer);

            //kontroll
            Assert.Null(result); //teenus lükkas objekti lisamise tagasi
            Assert.Empty(Svc<KeelteKoolV2Context>().Lecturers); //andmebaasi ei lisatud midagi
            Assert.Null(newLecturer.FirstName); //teenus ei pannud vaikeväärtusi
            Assert.Null(newLecturer.LastName);
            Assert.Equal("Bakalaureusekraad", newLecturer.Qualifications);
        }

        /// <summary>
        /// Kontrollib, et õppejõu loomisel salvestatakse kasutaja ID
        /// ning loomise ja muutmise aeg on loomise hetkel.
        /// </summary>
        [Fact]
        public async Task Should_SaveUserIdAndDates_WhenLecturerIsCreated()
        {
            //ülesseade
            LecturerDTO dto = MockLecturerDTOData();
            dto.UserID = "test-user-123";
            DateTime beforeCreate = DateTime.Now;

            //tegevus
            var result = await Svc<ILecturersServices>().Create(dto);
            DateTime afterCreate = DateTime.Now;

            //kontroll
            Assert.NotNull(result);
            Assert.Equal("test-user-123", result.UserID); //kasutaja id salvestati
            Assert.True(result.CreatedAt >= beforeCreate); //loomise aeg on õiges vahemikus
            Assert.True(result.CreatedAt <= afterCreate);
            Assert.True(result.ModifiedAt >= beforeCreate); //muutmise aeg on õiges vahemikus
            Assert.True(result.ModifiedAt <= afterCreate);
        }

        /// <summary>
        /// Kontrollib, et kahe õppejõu lisamisel saavad mõlemad erineva ID
        /// ja mõlemad leitakse andmebaasist üles.
        /// </summary>
        [Fact]
        public async Task Should_AddTwoLecturersWithDifferentIds_WhenCreatedTwice()
        {
            //ülesseade
            LecturerDTO secondDto = MockLecturerDTOData();
            secondDto.FirstName = "Teine";

            //tegevus
            Lecturer first = await AddObjectToDB();
            var second = await Svc<ILecturersServices>().Create(secondDto);
            var firstResult = await Svc<ILecturersServices>().DetailAsync(first.Id);
            var secondResult = await Svc<ILecturersServices>().DetailAsync(second.Id);

            //kontroll
            Assert.NotNull(second);
            Assert.NotEqual(first.Id, second.Id); //id-d on unikaalsed
            Assert.Equal(2, Svc<KeelteKoolV2Context>().Lecturers.Count()); //andmebaasis on 2 õppejõudu
            Assert.Equal("Test", firstResult.FirstName); //esimese andmed ei läinud teisega segamini
            Assert.Equal("Teine", secondResult.FirstName);
        }

        // ABIMEETODID

        /// <summary>
        /// Lisab andmebaasi testimiseks ühe õppejõu ja tagastab selle.
        /// </summary>
        private async Task<Lecturer> AddObjectToDB()
        {
            //tekitame uue objekti
            LecturerDTO lecturer = MockLecturerDTOData();
            //lisame andmebaasi
            var createdLecturer = await Svc<ILecturersServices>().Create(lecturer);
            return createdLecturer;
        }

        /// <summary>
        /// Loob testimiseks õppejõu DTO näidisandmed.
        /// </summary>
        private LecturerDTO MockLecturerDTOData()
        {
            return new LecturerDTO
            {
                FirstName = "Test",
                LastName = "Õppejõud",
                Qualifications = "Bakalaureusekraad"
            };
        }
    }
}
