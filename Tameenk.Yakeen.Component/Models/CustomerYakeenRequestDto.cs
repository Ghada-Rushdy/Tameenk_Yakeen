using System;


namespace YakeenComponent
{
    public class CustomerYakeenRequestDto
    {
        public readonly string ReferenceNumber;
        public string Nin { get; set; }
        public bool IsCitizen { get; set; }
        public string DateOfBirth { get; set; }

        public CustomerYakeenRequestDto()
        {
            ReferenceNumber = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 15);
        }
    }
}