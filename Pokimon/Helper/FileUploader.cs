namespace Pokimon.Helper
{
    public class FileUploader
    {
        public static string UploadFile(IFormFile file, string FolderName) {
            try
            {
         
               // 1 ) Get Directory

         string FolderPath =Path.Combine( Directory.GetCurrentDirectory() , FolderName );

               //2) Get File Name

         string FileName = Guid.NewGuid() + Path.GetFileName(file.FileName);

               // 3) Merge Path with File Name

         string FinalPath = Path.Combine(FolderPath, FileName);

               //4) Save File As Streams "Data Overtime"

         using (var Stream = new FileStream(FinalPath, FileMode.Create))

         {

         file.CopyTo(Stream);
         }
                return FileName;
         }
            catch (Exception e) {
                return e.Message;
         }

        }
    }
}
