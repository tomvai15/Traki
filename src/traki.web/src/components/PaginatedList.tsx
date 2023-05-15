import { Box, List, Pagination } from '@mui/material';
import React, { useEffect, useState } from 'react';
import SearchBar from './SearchBar';

interface Props<T> {
  items: T[],
  renderItem: (item: T) => React.ReactNode,
  height?: string,
  heightPerItem?: number
  selector?: (item: T) => string 
}

export function PaginatedList<T extends { id: number}>({items, renderItem, height, heightPerItem, selector}: Props<T>) {

  const itemsPerPage = 5;
  const [pageCount, setPageCount] = useState(1);
  const [currentPage, setCurrentPage] = useState(0);

  const [listHeight, setListHeight] = useState<string>();

  useEffect(() => {
    setPageCount(Math.ceil(items.length/itemsPerPage));
    
    if (heightPerItem) {
      const newHeight = `${items.length >= itemsPerPage ? heightPerItem * itemsPerPage : heightPerItem * items.length}px`;
      setListHeight(newHeight);
    } else if (height) {
      setListHeight(height);
    }

  }, [items, itemsPerPage]);

  const [searchQuery, setSearchQuery] = useState('');

  return (
    <Box>
      { selector && <SearchBar value={searchQuery} setValue={setSearchQuery}></SearchBar> }
      <List component="nav" style={{height: listHeight}}>
        {items.filter((item) => selector == undefined ? true : selector(item).toLowerCase().includes(searchQuery.toLowerCase()) ) .filter((item, index) => currentPage * itemsPerPage <= index && index < (currentPage + 1 ) * itemsPerPage).map((item, index) => <Box key={index}>{renderItem(item)}</Box>)}
      </List>
      <Pagination id='pagination' onChange={(e, value) => setCurrentPage(value-1)} count={pageCount} />
    </Box>
  );
}