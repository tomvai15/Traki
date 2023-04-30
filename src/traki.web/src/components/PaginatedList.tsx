import { Box, List, Pagination } from '@mui/material';
import React, { useEffect, useState } from 'react';

interface Props<T> {
  items: T[],
  renderItem: (item: T) => React.ReactNode,
  height?: string,
  heightPerItem?: number
}

export function PaginatedList<T extends { id: number}>({items, renderItem, height, heightPerItem}: Props<T>) {

  const [itemsPerPage, setItemsPerPage] = useState(5);
  const [pageCount, setPageCount] = useState(1);
  const [currentPage, setCurrentPage] = useState(0);

  const [listHeight, setListHeight] = useState<string>();

  useEffect(() => {
    console.log(items.length/itemsPerPage);
    console.log(Math.ceil(items.length/itemsPerPage));
    setPageCount(Math.ceil(items.length/itemsPerPage));
    
    if (heightPerItem) {
      console.log(heightPerItem * items.length);
      const newHeight = `${items.length >= itemsPerPage ? heightPerItem * itemsPerPage : heightPerItem * items.length}px`;
      console.log(newHeight);
      setListHeight(newHeight);
    } else if (height) {
      setListHeight(height);
    }

  }, [items, itemsPerPage]);

  return (
    <Box>
      <Pagination onChange={(e, value) => setCurrentPage(value-1)} count={pageCount} />
      <List component="nav" style={{height: listHeight}}>
        {items.filter((item, index) => currentPage * itemsPerPage <= index && index < (currentPage + 1 ) * itemsPerPage).map((item, index) => <Box key={index}>{renderItem(item)}</Box>)}
      </List>
    </Box>
  );
}