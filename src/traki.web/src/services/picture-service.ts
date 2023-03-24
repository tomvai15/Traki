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
}

export default new PictureService ();