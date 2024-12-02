namespace Microservices.Demo.TestService.Data;

public interface IMismatchRepository
{
    void AddMismatch(Mismatch mismatch);

    IReadOnlyCollection<Mismatch> ListAll();

    void Clear();
}
