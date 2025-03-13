using System.Runtime.CompilerServices;

class Papi
{
    public int Id { get; set; }
    public bool Infected = false;
    public bool WasInfected = false;
    public int InfCD = -1;
    public int InfStep = int.MaxValue;
    public List<int> Contacts = [];

    public Papi(int id)
    {
        this.Id = id;
    }

    public void Infect(Papi target, int curStep)
    {
        if (this.Infected && this.InfStep < curStep && !target.WasInfected)
        {
            target.GetInfected(curStep);
        }
    }

    public void GetInfected(int curStep)
    {
        this.Infected = true;
        this.InfCD = 8;
        this.InfStep = curStep;
        this.WasInfected = true;
    }

    public void InfTick()
    {
        if (this.Infected)
        {
            this.InfCD--;
            if (this.InfCD == 0)
            {
                this.Infected = false;
                this.WasInfected = true;
                this.InfCD = -1;
            }
        }
    }
}