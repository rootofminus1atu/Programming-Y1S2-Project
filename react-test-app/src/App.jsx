import { useState } from 'react'
import './App.css'
import Papa from 'papaparse';

function parseCSVFile(contents) {
  const parsedData = Papa.parse(contents, { header: true }).data;
  
  // Check if the last row is empty and remove it
  // do other checks too
  console.log(parsedData)

  const newData = parsedData
    .map((row) => {
      const lastName = row['LAST NAME'];
      const firstName = row['FIRST NAME'];
      const age = parseInt(row.AGE);

      // Check if any fields are missing
      if (!lastName || !firstName || !age) {
        return null; // Skip this row
      }

      return { lastName, firstName, age };
    })
    .filter((row) => row !== null)
  return newData;
}

function App() {
  const [uploadStatus, setUploadStatus] = useState("no file selected");
  const [fileData, setFileData] = useState([]);

  const handleFileUpload = (event) => {
    const file = event.target.files[0];
    const reader = new FileReader();

    reader.readAsText(file);

    reader.onloadstart = () => {
      setUploadStatus("loading");
    };

    reader.onload = (event) => {
      const contents = reader.result;
      const newData = parseCSVFile(contents);

      console.log(newData);
      setFileData(newData);
      setUploadStatus("file loaded");
    };

    reader.onerror = () => {
      setUploadStatus("failed to load");
    };
  };

  return (
    <div>
      <input type="file" onChange={handleFileUpload} accept=".csv" />
      <pre>{uploadStatus}</pre>
    </div>
  );
}

export default App;
