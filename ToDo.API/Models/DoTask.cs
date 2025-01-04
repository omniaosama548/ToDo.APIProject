namespace ToDo.API.Models
{
    public enum TasksStatus
    {
        New = 1,
        Process = 2,
        Completed = 3,
        Drafted = 4
    }
    public class DoTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartAt { get; set; } 
        public DateTime? EndAt { get; set; }   

        public string UserId { get; set; }  
        public User User { get; set; }
        public TasksStatus Status { get; set; } = TasksStatus.New;

        //                 status new=>1  process=>2   completed=>3 drafed=>4(id,num of status)
        //                create api to change staus in task =>use taskId &num of status 
        //              return id =>add ,update in response
        //{message:...................... ,
    //    data:{
    //    user:{email ,username,id
    //},
    //    token:''
    //    }
        
    //    }   //login reg
        //edit add (titlt,start at)=>reqired  (des,end at)=>optional
        //               in update,add,delete "delete sucessifulu" {"message":"delted successfuly"}
        // study errors handle
    }
}
