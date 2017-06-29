namespace BraudeTimetabler.Models
{
    public class AlgorithmInputsViewModel
    {
        //id: idList,
        //clashes: IsClashes,
        //freeDays: freeD,
        //maxGap: maxG
        public string[] Ids { get; set; }
        public bool Clashes { get; set; }
        public int FreeDays { get; set; }
        public int MaxGap { get; set; }
    }
}