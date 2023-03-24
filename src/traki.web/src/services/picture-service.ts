import axiosApiInstance from './axios-instance';

const route = 'control/folders/{folderName}/files/{fileName}';

class PictureService {
  async getPicture(folderName: string, fileName: string): Promise<string> {
    const fullRoute = route.format({folderName: folderName, fileName: fileName}); 
    const response = await axiosApiInstance.get(fullRoute, { headers: {}, responseType: "arraybuffer" });
    const base64 = btoa(new Uint8Array(response.data).reduce((data, byte) => data + String.fromCharCode(byte),''));
    const data = `data:${response.headers["content-type"]};base64,${base64}`;
    return data;
  }

  async uploadPicture(folderName: string, fileName: string, file: File): Promise<void> {
    const formData = new FormData();
    formData.append("file", file);
    const fullRoute = route.format({folderName: folderName, fileName: fileName}); 
    await axiosApiInstance.put(fullRoute, formData, {headers: {"Content-Type": "multipart/form-data"}});
  }
}

export default new PictureService ();