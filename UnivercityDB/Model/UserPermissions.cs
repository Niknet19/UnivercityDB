using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnivercityDB.Model
{
    public class Permission
    {
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }

    public class UserPermissions
    {
        public Permission ?EmployeePermission { get; set; }
        public Permission ?OrdersPermission { get; set; }
        public Permission ?CitiesPermission { get; set; }

        public Permission ?StreetsPermission { get; set; }

        public Permission? DocumentsPermission { get; set; }

        public Permission ?DepartmentsPermission { get; set; }

        public Permission ?EducationPermission { get; set; }

        public Permission ?AcademicDegreesPermission { get; set; }

        public Permission ?AcademicTitlesPermission { get; set; }

        public Permission ?JobtitlesPermission { get; set; }

        public Permission ?SpecialityPermission { get; set; }

        public UserPermissions()
        {
            OrdersPermission = new Permission();
            DocumentsPermission = new Permission();
            CitiesPermission = new Permission();
            StreetsPermission = new Permission();
            DepartmentsPermission = new Permission();
            EducationPermission = new Permission();
            AcademicDegreesPermission = new Permission();
            AcademicTitlesPermission = new Permission();
            JobtitlesPermission = new Permission();
            SpecialityPermission = new Permission();
            EmployeePermission = new Permission();
        }
    }
}
