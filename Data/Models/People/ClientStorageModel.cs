using System.Text.Json.Serialization;
using Domain.Entities;
using Domain.Entities.People;

namespace Data.Models.People;

[Serializable]
public class ClientStorageModel : IConverter<Client, ClientStorageModel>
{
    [JsonIgnore] public DateTime MeetTime;

    public ClientStorageModel()
    {
        Complaints = new string("complaints");
        Name = new string("name");
        Surname = new string("surname");
        Analyzes = new List<ReferenceForAnalysisStorageModel>();
        Appointments = new List<AppointmentStorageModel>();
        Id = new string("0");
        MeetTime = new DateTime(0);
    }

    public ClientStorageModel(string name, string surname, DateTime birthTime,
        IEnumerable<ReferenceForAnalysisStorageModel> analyses,
        IEnumerable<AppointmentStorageModel> appointments, string id, string complaints, DateTime meetTime)
    {
        MeetTime = meetTime;
        Name = new string(name);
        Surname = new string(surname);
        DateOfBirth = birthTime;
        Analyzes = new List<ReferenceForAnalysisStorageModel>(analyses);
        Appointments = new List<AppointmentStorageModel>(appointments);
        Id = id;
        Complaints = complaints;
    }

    public ClientStorageModel(string name, string surname, DateTime birthTime,
        IEnumerable<ReferenceForAnalysisStorageModel> analyses,
        string id)
    {
        Name = new string(name);
        Surname = new string(surname);
        DateOfBirth = birthTime;
        Analyzes = new List<ReferenceForAnalysisStorageModel>(analyses);
        Appointments = new List<AppointmentStorageModel>();
        Complaints = new string("complaints");
        MeetTime = new DateTime(0);
        Id = id;
    }

    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime DateOfBirth { get; set; }
    [JsonIgnore] public string Complaints { get; set; }
    [JsonIgnore] public IEnumerable<ReferenceForAnalysisStorageModel> Analyzes { get; set; }
    public string Id { get; set; }

    [field: NonSerialized] [JsonIgnore] public IEnumerable<AppointmentStorageModel> Appointments { get; set; }

    public Client ConvertToEntity(ClientStorageModel entity)
    {
        return new Client(entity.Name, entity.Surname, entity.DateOfBirth,
            entity.Analyzes.Select(d => d.ConvertToEntity(d)),
            entity.Appointments.Select(a => a.ConvertToEntity(a)), entity.Id, entity.Complaints, entity.MeetTime);
    }

    public ClientStorageModel ConvertToStorageEntity(Client entity)
    {
        var appointments = entity.Appointments;


        return new ClientStorageModel(entity.Name, entity.Surname, entity.DateOfBirth,
            from referenceForAnalysis
                in entity.Analyzes
            let a = referenceForAnalysis.Analysis
            select new ReferenceForAnalysisStorageModel(
                new AnalysisStorageModel(a.Title, a.TimeForPrepearing, a.TimeForTaking, a.Id),
                referenceForAnalysis.AnalysisTime,
                referenceForAnalysis.ClientId),
            appointments.Select(appointment =>
                new AppointmentStorageModel(
                    appointment.DoctorLogin,
                    appointment.ClientId,
                    appointment.MeetTime,
                    appointment.ClientComplaints))
            , entity.Id, entity.Complaints, entity.MeetTime);
    }

    public override string ToString()
    {
        return string.Join(" ",
            Name,
            Surname,
            MeetTime.Equals(new DateTime(0))
                ? "\nДата рождения: " + DateOfBirth.ToString("MM/dd/yyyy")
                : "\nВремя записи: " + MeetTime);
    }
}