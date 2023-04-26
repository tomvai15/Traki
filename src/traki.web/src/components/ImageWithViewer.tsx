import { Box } from '@mui/material';
import  React, { useState } from 'react';
import Lightbox from "yet-another-react-lightbox";
import "yet-another-react-lightbox/styles.css";
import Zoom from "yet-another-react-lightbox/plugins/zoom";

type ImageWithViewerProps = {
  width?: number,
  height?: number,
  source?: string
}

export default function ImageWithViewer({ width, height, source }: ImageWithViewerProps) {
  const [viewerActive, setViewerActive] = useState<boolean>(false);

  return (
    <Box>
      { source && 
        <img onClick={() => setViewerActive(true)}
          style={{borderRadius: '10px'}}
          height={height}
          width='auto'
          src={source}
          alt="random"
        />}
      { source && viewerActive && (
        <Lightbox
          plugins={[Zoom]}
          carousel={{finite: true}}
          open={viewerActive}
          close={() => setViewerActive(false)}
          slides={[
            { src: source},
          ]}
        />
      )}
    </Box>
  );
}
