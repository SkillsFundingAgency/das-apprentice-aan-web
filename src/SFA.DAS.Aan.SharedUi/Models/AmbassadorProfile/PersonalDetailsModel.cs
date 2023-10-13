﻿namespace SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
public class PersonalDetailsModel
{
    public PersonalDetailsModel(string fullName, string regionName, MemberUserType userType)
    {
        FullName = fullName;
        RegionName = regionName;
        UserType = userType;
    }

    public string FullName { get; set; }
    public string RegionName { get; set; }
    public MemberUserType UserType { get; set; }
}