import { Box, List, Pagination } from '@mui/material';
import React, { useEffect, useState } from 'react';

interface Props<T> {
  items: T[],
  renderItem: (item: T) => React.ReactNode,
  height?: string
}

export function PaginatedList<T extends { id: number}>({items, renderItem, height}: Props<T>) {

  const [itemsPerPage, setItemsPerPage] = useState(5);
  const [pageCount, setPageCount] = useState(1);
  const [currentPage, setCurrentPage] = useState(0);

  useEffect(() => {
    console.log(items.length/itemsPerPage);
    console.log(Math.ceil(items.length/itemsPerPage));
    setPageCount(Math.ceil(items.length/itemsPerPage));

  }, [items]);

  return (
    <Box>
      <Pagination onChange={(e, value) => setCurrentPage(value-1)} count={pageCount} />
      <List component="nav" style={{height: height}}>
        {items.filter((item, index) => currentPage * itemsPerPage <= index && index < (currentPage + 1 ) * itemsPerPage).map((item, index) => <Box key={index}>{renderItem(item)}</Box>)}
      </List>
    </Box>
  );
}