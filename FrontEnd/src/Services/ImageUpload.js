import axios from 'axios';

export default async function ImageUpload(file, url) {
  return new Promise(async (resolve, reject) => {
    try {
      const formData = new FormData();
      formData.append('file', file);

      const res = await axios.post(url, formData);

      if (res.status === 200) {
        const data = res.data.imageUrl;
        resolve(data); 
      } else {
        reject('Error uploading the image.'); 
      }
    } catch (err) {
      console.error(err);
      reject(err); 
    }
  });
}
